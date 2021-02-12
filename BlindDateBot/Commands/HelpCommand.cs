using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Contexts;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class HelpCommand : IBotCommand
    {
        public string Name => "/help";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            logger.LogDebug("Help command was initiated by {username}({userid})", message.From.Username, message.From.Id);

            await botClient.SendTextMessageAsync(message.From.Id, Messages.HelpMessage);       
        }
    }
}
