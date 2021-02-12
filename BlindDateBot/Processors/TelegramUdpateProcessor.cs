using System;
using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;
using BlindDateBot.Models.Enums;

using Microsoft.EntityFrameworkCore;
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
        private readonly SqlServerContext _db;

        private readonly TransactionProcessor _transactionsProcessor;

        public TelegramUdpateProcessor(ITelegramBotClient botClient, ILogger logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            _botClient = botClient;
            _db = new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]);

            _transactionsProcessor = new TransactionProcessor(botClient, logger);

        }

        public async void Process(Update update)
        {
            _logger.LogDebug("Update {updateId} received. Update type is {updateType}",
                             update.Id, update.Type.ToString());
            try
            {
                if (update.Type == UpdateType.Message)
                {
                    await ProcessMessage(update.Message);
                }
                else if (update.Type == UpdateType.CallbackQuery)
                {
                    await ProcessCallbackQuery(update.CallbackQuery);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} | Stack trace: {ex.StackTrace}");
            }
        }

        private async Task ProcessMessage(Message message)
        {
            if (!await IsMessageValidIfNotInformUser(message))
            {
                return;
            }

            object userTransaction = GetUserTransaction(message);

            var strategy = TransactionProcessStrategy.Default;
            if (message.Text?.StartsWith("/") == true)
            {
                strategy = TransactionProcessStrategy.Command;
                userTransaction = new CommandTransactionModel(message.From.Id);
            }
            else if (userTransaction != null)
            {
                strategy = SelectStrategy(userTransaction as TransactionBaseModel);
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

        private async void InformUser(int userTelegramId, string message)
        {
            await _botClient.SendTextMessageAsync(userTelegramId, message);
        }

        private async Task ProcessCallbackQuery(CallbackQuery query)
        {
            var message = query.Message;
            message.Text = query.Data;
            message.From.Id = (int)message.Chat.Id;

            await ProcessMessage(message);
        }

        private async Task<bool> IsMessageValidIfNotInformUser(Message message)
        {
            if (message == null)
            {
                return false;
            }
            
            if (await IsUserBlock(message.From.Id))
            {
                InformUser(message.From.Id, Messages.YourAccountBlocked);
                return false;
            }

            return true;
        }

        private async Task<bool> IsUserBlock(int userTelegramId)
        {
            return (await _db.Users.FirstOrDefaultAsync(u => u.TelegramId == userTelegramId))?.IsBlocked == true;
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

        private static TransactionProcessStrategy SelectStrategy(TransactionBaseModel transaction)
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
