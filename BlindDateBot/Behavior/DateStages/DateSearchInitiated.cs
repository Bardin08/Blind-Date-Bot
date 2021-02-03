using System.Linq;
using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Domain.Models;
using BlindDateBot.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Behavior.DateStages
{
    public class DateSearchInitiated : Interfaces.IDateTransactionState
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            var currentTransaction = transaction as DateTransactionModel;

            await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.DateSearchText);

            var user = await db.Users.FirstOrDefaultAsync(u => u.TelegramId == currentTransaction.RecepientId);
            if (user == null)
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.InternalErrorUserNotFound);
                return;
            }

            var interlocutor = await db.Users.FirstOrDefaultAsync(u => u.IsFree == true
                                                                       && user.InterlocutorGender == u.Gender
                                                                       && user.Gender == u.InterlocutorGender
                                                                       && u.Id != user.Id);
            if (interlocutor == null)
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.InterlocutorNotFound);
                return;
            }

            user.IsFree = false;
            db.Update(user);

            interlocutor.IsFree = false;
            db.Update(interlocutor);

            var match = new DateModel(currentTransaction.TransactionId)
            {
                FirstUser = user,
                SecondUser = interlocutor,
                IsActive = true
            };

            await db.Dates.AddAsync(match);
            await db.SaveChangesAsync();

            await botClient.SendTextMessageAsync(user.TelegramId, Messages.DateHasBegan);
            await botClient.SendTextMessageAsync(interlocutor.TelegramId, Messages.DateHasBegan);
        }
    }
}
