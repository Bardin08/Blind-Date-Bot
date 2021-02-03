using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BlindDateBot.Commands;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;
using BlindDateBot.Options;

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
        private readonly SqlServerContext _db;

        private IMessageProcessingStrategy _transactionsProcessor;
        private List<object> _transactions;


        public BlindDateBot(IBlindDateBotClient botClient, ILogger<BlindDateBot> logger, SqlServerContext db)
        {
            _botClient = botClient.BotClient;
            _options = botClient.Options;

            _logger = logger;
            _db = db;

            _transactionsProcessor = new Strategies.CommandProcessStrategy();
            _transactions = new();
        }

        public void Execute()
        {
            _botClient.OnMessage += MessageReceived;
            _botClient.OnCallbackQuery += CallbackQueryReceived;

            StartCommand.RegistrationInitiated += RegistrationInitiated;

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

        private async void RegistrationInitiated(RegistrationTransactionModel t)
        {
            _transactions.Add(t);

            _transactionsProcessor = new Strategies.RegistrationProcessStrategy();
            await _transactionsProcessor.ProcessTransaction(new Message(), t, _botClient, _logger, _db);
        }

        private async void MessageReceived(object sender, MessageEventArgs e)
        {
            await ProcessMessage(e.Message);
        }

        private async Task ProcessMessage(Message message)
        {
            RemoveCompletedTransactions();
            object userTransaction = _transactions.Find(transaction => (transaction as TransactionBaseModel)?.RecepientId == message.From.Id);

            if (message.Text?.StartsWith("/") == true)
            {
                _transactionsProcessor = new Strategies.CommandProcessStrategy();
                userTransaction = new CommandTransactionModel(message.From.Id);
            }
            else if (userTransaction != null)
            {
                _transactionsProcessor = SelectStrategy(userTransaction as TransactionBaseModel);
            }

            await _transactionsProcessor.ProcessTransaction(message, userTransaction, _botClient, _logger, _db);
        }

        private static IMessageProcessingStrategy SelectStrategy(TransactionBaseModel transaction)
        {
            return (transaction.TransactionType) switch
            {
                Models.Enums.TransactionType.DataMessaging => new Strategies.RegistrationProcessStrategy(),
                Models.Enums.TransactionType.Command => new Strategies.CommandProcessStrategy(),
                Models.Enums.TransactionType.Registration => new Strategies.RegistrationProcessStrategy(),
                _ => throw new NotImplementedException(),    
            };
        }

        private void RemoveCompletedTransactions()
        {
            _transactions.RemoveAll(t => (t as TransactionBaseModel).IsComplete == true);
        }
    }
}