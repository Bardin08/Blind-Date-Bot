using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;
using BlindDateBot.Models.Enums;
using BlindDateBot.Strategies;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Processors
{
    public class TransactionProcessor
    {
        private readonly ILogger _logger;

        private readonly ITelegramBotClient _botClient;

        private TransactionProcessStrategy _strategy;
        private readonly Dictionary<TransactionProcessStrategy, ITransactionProcessStrategy> _strategies;


        public TransactionProcessor(ITelegramBotClient botClient, ILogger logger)
            : this(TransactionProcessStrategy.Default, botClient, logger)
        {
        }

        public TransactionProcessor(TransactionProcessStrategy strategy, ITelegramBotClient botClient, ILogger logger)
        {
            _logger = logger;
            _botClient = botClient;

            _strategy = strategy;
            _strategies = new();
        }

        public TransactionProcessStrategy Strategy 
        {
            set 
            {
                _strategy = value;
            }
        }

        public async Task ProcessTransaction(Message message, object transaction, SqlServerContext db)
        {
            var currentTransaction = transaction as TransactionBaseModel;

            await RemovePreviousMessages(message.From.Id, currentTransaction);

            await SelectStrategy().ProcessTransaction(message, transaction, _botClient, _logger, db);

            _logger.LogDebug("Transaction {transactionId} is processing as {transactionType}",
                             currentTransaction.TransactionId, currentTransaction.TransactionType.ToString());
        }

        private async Task RemovePreviousMessages(int chatId, TransactionBaseModel currentTransaction)
        {
            foreach (var id in currentTransaction.MessageIds)
            {
                await _botClient.DeleteMessageAsync(chatId, id);
            }
            currentTransaction.MessageIds.Clear();
        }

        private ITransactionProcessStrategy SelectStrategy()
        {
            ITransactionProcessStrategy strategy = null;

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
