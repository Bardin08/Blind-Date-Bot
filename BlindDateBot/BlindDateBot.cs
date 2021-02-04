using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BlindDateBot.Commands;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Delegates;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;
using BlindDateBot.Models.Enums;
using BlindDateBot.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace BlindDateBot
{
    public class BlindDateBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BlindDateBotOptions _options;

        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        private TransactionProcessor _transactionsProcessor;

        private List<object> _transactions;
        private List<DateTransactionModel> _dates;


        public BlindDateBot(IBlindDateBotClient botClient, ILogger<BlindDateBot> logger, IConfiguration config)
        {
            _botClient = botClient.BotClient;
            _options = botClient.Options;

            _logger = logger;
            _config = config;

            _transactions = new();
            _transactionsProcessor = new();
            _dates = new();
        }

        public void Execute()
        {
            _botClient.OnMessage += MessageReceived;
            _botClient.OnCallbackQuery += CallbackQueryReceived;

            StartCommand.RegistrationInitiated += RegistrationInitiated;
            NextDateCommand.DateFound += DateFound;

            _botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private async void CallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            var message = e.CallbackQuery.Message;
            message.Text = e.CallbackQuery.Data;
            message.From.Id = (int)message.Chat.Id;

            await ProcessMessage(message);
        }

        private void DateFound(DateTransactionModel transaction)
        {
            if (transaction != null)
            {
                _dates.Add(transaction);
            }
        }

        private async void RegistrationInitiated(RegistrationTransactionModel transaction)
        {
            _transactions.Add(transaction);

            ExecuteTransactionProcessing(null,
                                         transaction,
                                         TransactionProcessStrategy.Registration);
        }

        private async void ExecuteTransactionProcessing(Message message, object transaction, TransactionProcessStrategy strategy)
        {
            _transactionsProcessor.Strategy = strategy;
            await _transactionsProcessor.ProcessTransaction(message,
                                                            transaction,
                                                            _botClient,
                                                            _logger,
                                                            new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));
        }

        private async void MessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Message != null)
            {
                await ProcessMessage(e.Message);
            }
        }

        private async Task ProcessMessage([NotNull] Message message)
        {
            RemoveCompletedTransactions();
            object userTransaction = _transactions.Find(transaction => (transaction as TransactionBaseModel)?.RecepientId == message.From.Id);

            if (userTransaction is null)
            {
                userTransaction = _dates.Find(t => t.Date.FirstUser.TelegramId == message.From.Id 
                                                || t.Date.SecondUser.TelegramId == message.From.Id);
            }

            var strategy = TransactionProcessStrategy.Default;

            if (message.Text?.StartsWith("/") == true)
            {
                _transactions.RemoveAll(x => (x as TransactionBaseModel).RecepientId == (userTransaction as TransactionBaseModel)?.RecepientId);

                strategy = TransactionProcessStrategy.Command;
                userTransaction = new CommandTransactionModel(message.From.Id);
            }
            else if (userTransaction != null)
            {
                strategy = SelectStrategy(userTransaction as TransactionBaseModel);
            }

            if (userTransaction != null)
            {
                ExecuteTransactionProcessing(message,
                                             userTransaction,
                                             strategy);
            }
        }

        private static TransactionProcessStrategy SelectStrategy(TransactionBaseModel transaction)
        {
            return (transaction.TransactionType) switch
            {
                TransactionType.DateMessaging => TransactionProcessStrategy.Date,
                TransactionType.Command => TransactionProcessStrategy.Command,
                TransactionType.Registration => TransactionProcessStrategy.Registration,
                _ => TransactionProcessStrategy.Default
            };
        }

        private void RemoveCompletedTransactions()
        {
            _transactions.RemoveAll(t => (t as TransactionBaseModel).IsComplete == true);
        }
    }
}