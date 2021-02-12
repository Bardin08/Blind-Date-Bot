using System;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;
using BlindDateBot.Models.Enums;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BlindDateBot.Processors
{
    public class TelegramUdpateProcessor
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        private readonly ITelegramBotClient _botClient;

        private readonly TransactionProcessor _transactionsProcessor;

        public TelegramUdpateProcessor(ITelegramBotClient botClient, ILogger logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            _botClient = botClient;

            _transactionsProcessor = new TransactionProcessor(botClient, logger);
        }

        public void Process(Update update)
        {
            _logger.LogDebug("Update {updateId} received. Update type is {updateType}",
                             update.Id, update.Type.ToString());
            try
            {
                if (update.Type == UpdateType.Message)
                {
                    ProcessMessage(update.Message);
                }
                else if (update.Type == UpdateType.CallbackQuery)
                {
                    ProcessCallbackQuery(update.CallbackQuery);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} | Stack trace: {ex.StackTrace}");
            }
        }

        private void ProcessMessage(Message message)
        {
            if (!IsMessageValidIfNotInformUser(message))
            {
                return;
            }

            object userTransaction = GetUserTransaction(message);

            var strategy = TransactionProcessStrategy.Default;
            if (message.Text?.StartsWith("/") == true)
            {
                strategy = TransactionProcessStrategy.Command;
                userTransaction = new CommandTransactionModel(message);
            }
            else if (userTransaction != null)
            {
                strategy = SelectStrategy(userTransaction as BaseTransactionModel);
            }

            if (userTransaction != null)
            {
                ExecuteTransactionProcessing(message, userTransaction, strategy);
            }
            else
            {
                InformUser(message.From.Id, Messages.YouHaventAnActiveDate);
            }
        }

        private void ProcessCallbackQuery(CallbackQuery query)
        {
            var message = query.Message;
            message.Text = query.Data;
            message.From.Id = (int)message.Chat.Id;

            ProcessMessage(message);
        }

        private async void InformUser(int userTelegramId, string message)
        {
            await _botClient.SendTextMessageAsync(userTelegramId, message);
        }

        private static bool IsMessageValidIfNotInformUser(Message message)
        {
            return message != null;
        }

        private static object? GetUserTransaction(Message message)
        {
            object userTransaction = TransactionsContainer.GetTransactionByRecipientId(message.From.Id);

            if (userTransaction == null)
            {
                userTransaction = TransactionsContainer.GetDateTransactionByRecipientId(message.From.Id);
            }

            return userTransaction;
        }

        private async void ExecuteTransactionProcessing(Message message, object transaction, TransactionProcessStrategy strategy)
        {
            try
            {
                _transactionsProcessor.Strategy = strategy;
                await _transactionsProcessor.ProcessTransaction(message, transaction,
                    new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} | Stack trace: {ex.StackTrace}");
            }
        }

        private static TransactionProcessStrategy SelectStrategy(BaseTransactionModel transaction)
        {
            return (transaction.TransactionType) switch
            {
                TransactionType.DateMessaging => TransactionProcessStrategy.Date,
                TransactionType.Command => TransactionProcessStrategy.Command,
                TransactionType.Registration => TransactionProcessStrategy.Registration,
                TransactionType.Feedback => TransactionProcessStrategy.Feedback,
                TransactionType.Report => TransactionProcessStrategy.Report,
                _ => TransactionProcessStrategy.Default
            };
        }
    }
}
