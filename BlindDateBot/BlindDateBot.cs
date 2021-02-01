using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BlindDateBot.Commands;
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

        private IMessageProcessingStrategy _transactionProcessor;
        private List<object> _transactions;


        public BlindDateBot(IBlindDateBotClient botClient, ILogger<BlindDateBot> logger)
        {
            _botClient = botClient.BotClient;
            _options = botClient.Options;

            _logger = logger;

            _transactionProcessor = new Strategies.CommandProcessStrategy();
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

            _transactionProcessor = new Strategies.RegistrationProcessStrategy();
            await _transactionProcessor.ProcessTransaction(new Message(), t, _botClient, _logger, null);
        }

        private async void MessageReceived(object sender, MessageEventArgs e)
        {
            await ProcessMessage(e.Message);
        }

        private async Task ProcessMessage(Message message)
        {
            RemoveCompletedTransactions();

            var t = _transactions.FirstOrDefault(x => (x as TransactionBaseModel).RecepientId == message.From.Id);

            if (t != null)
                await _transactionProcessor.ProcessTransaction(message, t, _botClient, _logger, null);
            else
                await _transactionProcessor.ProcessTransaction(message, new CommandTransactionModel(message.From.Id), _botClient, _logger, null);
        }

        private void RemoveCompletedTransactions()
        {
            _transactions.RemoveAll(t => (t as TransactionBaseModel).IsComplete == true);
        }
    }
}
