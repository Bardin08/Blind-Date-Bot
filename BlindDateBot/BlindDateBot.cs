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
using BlindDateBot.Strategies;

using Microsoft.EntityFrameworkCore;
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

            LoadDatesFromDb();
        }

        public void Execute()
        {
            _botClient.OnMessage += MessageReceived;
            _botClient.OnCallbackQuery += CallbackQueryReceived;

            StartCommand.RegistrationInitiated += RegistrationInitiated;
            NextDateCommand.DateInitiated += DateInitiated;

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

        private async void DateInitiated(DateTransactionModel transaction)
        {
            RemoveCompletedTransactions();

            _transactions.Add(transaction);

            _transactionsProcessor = new DateProcessStrategy();
            await _transactionsProcessor.ProcessTransaction(null, transaction, _botClient, _logger, _db);
        }

        private async void RegistrationInitiated(RegistrationTransactionModel transaction)
        {
            _transactions.Add(transaction);

            _transactionsProcessor = new RegistrationProcessStrategy();
            await _transactionsProcessor.ProcessTransaction(null, transaction, _botClient, _logger, _db);
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
                _transactions.RemoveAll(x => (x as TransactionBaseModel).RecepientId == (userTransaction as TransactionBaseModel)?.RecepientId);

                _transactionsProcessor = new CommandProcessStrategy();
                userTransaction = new CommandTransactionModel(message.From.Id);
            }
            else if (userTransaction != null)
            {
                _transactionsProcessor = SelectStrategy(userTransaction as TransactionBaseModel);
            }

            if (userTransaction != null)
            {
                await _transactionsProcessor.ProcessTransaction(message, userTransaction, _botClient, _logger, _db);
            }
        }

        private static IMessageProcessingStrategy SelectStrategy(TransactionBaseModel transaction)
        {
            return (transaction.TransactionType) switch
            {
                Models.Enums.TransactionType.DataMessaging => new DateProcessStrategy(),
                Models.Enums.TransactionType.Command => new CommandProcessStrategy(),
                Models.Enums.TransactionType.Registration => new RegistrationProcessStrategy(),
                _ => throw new NotImplementedException(),
            };
        }

        private void RemoveCompletedTransactions()
        {
            _transactions.RemoveAll(t => (t as TransactionBaseModel).IsComplete == true);
        }

        private void LoadDatesFromDb()
        {
            var dates = _db.Dates
                .Include(d => d.FirstUser)
                .Include(d => d.SecondUser).ToList();

            foreach (var date in dates)
            {
                _transactions.Add(new DateTransactionModel(date.FirstUser.TelegramId)
                {
                    State = new Behavior.DateStages.DateSearchInitiated(),
                    IsComplete = !date.IsActive,
                    Messages = date.Messages,
                    TransactionId = date.Id,
                });
                _transactions.Add(new DateTransactionModel(date.SecondUser.TelegramId)
                {
                    State = new Behavior.DateStages.DateSearchInitiated(),
                    IsComplete = !date.IsActive,
                    Messages = date.Messages,
                    TransactionId = date.Id,
                });
            }
        }
    }
}