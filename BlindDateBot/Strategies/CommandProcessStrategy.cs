using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Strategies
{
    public class CommandProcessStrategy : IMessageProcessingStrategy
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient)
        {
            if (message.Text == null)
            {
                await botClient.SendTextMessageAsync(message.From.Id, Messages.CommandNotFoundMessage);
                return;
            }

            var commands = LoadCommands();

            var requiredCommand = commands?.Find(c => c.Name == message.Text);

            requiredCommand?.Execute(message, transaction as CommandTransaction, botClient);
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
