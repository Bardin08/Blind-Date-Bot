using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Strategies
{
    public class CommandProcessStrategy : ITransactionProcessStrategy
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as CommandTransactionModel;

            if (currentTransaction.Message?.Text == null)
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.CommandNotFoundMessage);
                return;
            }

            var commands = LoadCommands();

            var requiredCommand = commands?.Find(c => c.Name == currentTransaction.Message.Text);

            if (requiredCommand != null)
            {
                await requiredCommand.Execute(currentTransaction, botClient, logger, db);
            }
            else
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.CommandNotFoundMessage);
            }
        }

        private static List<IBotCommand> LoadCommands()
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
