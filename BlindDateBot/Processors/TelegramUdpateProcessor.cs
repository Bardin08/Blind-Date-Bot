using System;
using System.Threading.Tasks;

using BlindDateBot.Commands;
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

            _transactionsProcessor = new(botClient, logger, config);

            NextDateCommand.DateFound += DateFound;
            EndDateCommand.DateEnd += DateEnd;
        }

        private void DateEnd(string dateId)
        {
            TransactionsContainer.RemoveDate(dateId);
        }

        private void DateFound(DateTransactionModel transaction)
        {
            TransactionsContainer.AddDate(transaction);
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
            if (message == null)
            {
                return;
            }

            object userTransaction = TransactionsContainer.GetTransactionByRecipientId(message.From.Id);

            if (userTransaction is null)
            {
                userTransaction = TransactionsContainer.GetDateTransactionByRecipientId(message.From.Id);
            }

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
                ExecuteTransactionProcessing(message,
                                             userTransaction,
                                             strategy);
            }
            else
            {
                await _botClient.SendTextMessageAsync(message.From.Id, Messages.YouHaventAnActiveDate);
            }
        }

        private async Task ProcessCallbackQuery(CallbackQuery query)
        {
            var message = query.Message;
            message.Text = query.Data;
            message.From.Id = (int)message.Chat.Id;

            await ProcessMessage(message);
        }

        private async void ExecuteTransactionProcessing(Message message, object transaction, TransactionProcessStrategy strategy)
        {
            try
            {
                _transactionsProcessor.Strategy = strategy;
                await _transactionsProcessor.ProcessTransaction(message,
                                                                transaction,
                                                                _botClient,
                                                                _logger,
                                                                new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
