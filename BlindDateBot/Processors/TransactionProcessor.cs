using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BlindDateBot.Commands;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;
using BlindDateBot.Models.Enums;
using BlindDateBot.Strategies;

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
            foreach (var id in (transaction as TransactionBaseModel).MessageIds)
            {
                await botClient.DeleteMessageAsync(message.Chat.Id, id);
            }
            (transaction as TransactionBaseModel).MessageIds.Clear();

            await SelectStrategy().ProcessTransaction(message, transaction, botClient, logger, db);
        }

        private async void RegistrationInitiated(RegistrationTransactionModel transaction)
        {
            TransactionsContainer.AddTransaction(transaction);
            Strategy = TransactionProcessStrategy.Registration;

            await ProcessTransaction(null,
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
                    _ => throw new ArgumentException("Incorrect value.", nameof(Strategy)),
                };

                _strategies.Add(_strategy, strategy);
            }

            return strategy ?? _strategies[_strategy];
        }
    }
}
