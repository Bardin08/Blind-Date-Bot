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

namespace BlindDateBot
{
    public class TransactionProcessor
    {
        private Dictionary<TransactionProcessStrategy, ITransactionProcessingStrategy> _strategies;

        private TransactionProcessStrategy _strategy;
        public TransactionProcessStrategy Strategy 
        {
            set
            {
                _strategy = value;
            }
        }
        
        public TransactionProcessor()
        {
            _strategy = TransactionProcessStrategy.Default;
            _strategies = new();
        }

        public TransactionProcessor(TransactionProcessStrategy strategy)
        {
            _strategy = strategy;
            _strategies = new();
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
