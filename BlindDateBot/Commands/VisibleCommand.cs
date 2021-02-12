using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Domain.Models;
using BlindDateBot.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Commands
{
    public class VisibleCommand : IBotCommand
    {
        public string Name => "/visible";

        public async Task Execute(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as BaseTransactionModel;

            logger.LogDebug("User {username}({userId}) was changed its visible", currentTransaction.Message.From.Username, currentTransaction.Message.From.Id);

            var user = await db.Set<UserModel>().FirstOrDefaultAsync(u => u.TelegramId == currentTransaction.RecipientId);

            user.IsVisible = !user.IsVisible;

            db.Update(user);
            await db.SaveChangesAsync();
        }
    }
}
