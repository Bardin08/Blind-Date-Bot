using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class VisibleCommand : IBotCommand
    {
        public string Name => "/visible";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            var currentTransaction = transaction as TransactionBaseModel;

            logger.LogDebug("User {username}({userId}) was changed its visible", message.From.Username, message.From.Id);

            var user = await db.Users.FirstOrDefaultAsync(u => u.TelegramId == currentTransaction.RecipientId);

            user.IsVisible = !user.IsVisible;

            db.Update(user);
            await db.SaveChangesAsync();
        }
    }
}
