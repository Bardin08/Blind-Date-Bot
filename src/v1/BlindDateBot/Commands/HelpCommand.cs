using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Commands
{
    public class HelpCommand : IBotCommand
    {
        public string Name => "/help";

        public async Task Execute(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as BaseTransactionModel;

            logger.LogDebug("Help command was initiated by {username}({userid})",
                            currentTransaction.Message.From.Username,
                            currentTransaction.Message.From.Id);

            await botClient.SendTextMessageAsync(currentTransaction.Message.From.Id, Messages.HelpMessage);       
        }
    }
}
