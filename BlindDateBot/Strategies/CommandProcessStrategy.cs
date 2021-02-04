using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Data.Interfaces;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Strategies
{
    public class CommandProcessStrategy : ITransactionProcessingStrategy
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            var currentTransaction = transaction as CommandTransactionModel;

            if (message?.Text == null)
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.CommandNotFoundMessage);
                return;
            }

            var commands = LoadCommands();

            var requiredCommand = commands?.Find(c => c.Name == message.Text);

            if (requiredCommand != null)
            {
                await requiredCommand.Execute(message, currentTransaction, botClient, logger, db);
            }
            else
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.CommandNotFoundMessage);
            }
        }

        private List<IBotCommand> LoadCommands()
        {
            var commands = new List<IBotCommand>();
            var foundCommands = Assembly.GetExecutingAssembly().GetTypes()
                .Where(types => types.IsClass && !types.IsAbstract
                && types.GetInterface("IBotCommand") != null).ToList();

            foreach (var command in foundCommands)
            {
                commands.Add((IBotCommand)Activator.CreateInstance(command));
            }

            return commands;
        }
    }
}
