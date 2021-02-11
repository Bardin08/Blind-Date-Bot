using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BlindDateBot.Commands;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;
using BlindDateBot.Models.Enums;
using BlindDateBot.Strategies;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Processors
{
    public class TransactionProcessor
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        private readonly ITelegramBotClient _botClient;

        private TransactionProcessStrategy _strategy;
        private readonly Dictionary<TransactionProcessStrategy, ITransactionProcessingStrategy> _strategies;


        public TransactionProcessor(ITelegramBotClient botClient, ILogger logger, IConfiguration config)
            : this(TransactionProcessStrategy.Default, botClient, logger, config)
        {
        }

        public TransactionProcessor(TransactionProcessStrategy strategy, ITelegramBotClient botClient, ILogger logger, IConfiguration config)
        {
            _logger = logger;
            _botClient = botClient;
            _config = config;


            _strategy = strategy;
            _strategies = new();

            StartCommand.RegistrationInitiated += RegistrationInitiated;
            FeedbackCommamd.FeedbackTransactionInitiated += FeedbackTransactionInitiated;
            ReportCommand.ReportInitiated += ReportInitiated;
        }

        public TransactionProcessStrategy Strategy 
        {
            set
            {
                _strategy = value;
            }
        }

        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            if ((await db.Users.FirstOrDefaultAsync(u => u.TelegramId == message.From.Id))?.IsBlocked == true)
            {
                await botClient.SendTextMessageAsync(message.From.Id, Messages.YourAccountIsBlocked);
                return;
            }

            var currentTransaction = transaction as TransactionBaseModel;

            foreach (var id in currentTransaction.MessageIds)
            {
                await botClient.DeleteMessageAsync(message.Chat.Id, id);
            }
            currentTransaction.MessageIds.Clear();

            await SelectStrategy().ProcessTransaction(message, transaction, botClient, logger, db);

            _logger.LogDebug("Transaction {transactionId} is processing as {transactionType}",
                             currentTransaction.TransactionId, currentTransaction.TransactionType.ToString());
        }

        private async void RegistrationInitiated(RegistrationTransactionModel transaction)
        {
            TransactionsContainer.AddTransaction(transaction);
            Strategy = TransactionProcessStrategy.Registration;

            await ProcessTransaction(new Message() { From = new User { Id = transaction.RecipientId } },
                                     transaction,
                                     _botClient,
                                     _logger,
                                     new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));
        }

        private async void FeedbackTransactionInitiated(FeedbackTransactionModel transaction)
        {
            TransactionsContainer.AddTransaction(transaction);
            Strategy = TransactionProcessStrategy.Feedback;

            await ProcessTransaction(new Message() { From = new User { Id = transaction.RecipientId } },
                                     transaction,
                                     _botClient,
                                     _logger,
                                      new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));
        }

        private async void ReportInitiated(ReportTransactionModel transaction)
        {
            TransactionsContainer.AddTransaction(transaction);
            Strategy = TransactionProcessStrategy.Report;

            await ProcessTransaction(new Message() { From = new User { Id = transaction.RecipientId } },
                                     transaction,
                                     _botClient,
                                     _logger,
                                     new SqlServerContext(_config["DB:MsSqlDb:ConnectionString"]));
        }

        private ITransactionProcessingStrategy SelectStrategy()
        {
            ITransactionProcessingStrategy strategy = null;

            if (!_strategies.ContainsKey(_strategy))
            {
                strategy = _strategy switch
                {
                    TransactionProcessStrategy.Default => new DefaultProcessStrategy(),
                    TransactionProcessStrategy.Command => new CommandProcessStrategy(),
                    TransactionProcessStrategy.Registration => new RegistrationProcessStrategy(),
                    TransactionProcessStrategy.Date => new DateProcessStrategy(),
                    TransactionProcessStrategy.Feedback => new FeedbackProcessStrategy(),
                    TransactionProcessStrategy.Report => new ReportProcessStrategy(),
                    _ => throw new ArgumentException("Incorrect value.", nameof(Strategy)),
                };

                _strategies.Add(_strategy, strategy);
            }

            return strategy ?? _strategies[_strategy];
        }
    }
}
