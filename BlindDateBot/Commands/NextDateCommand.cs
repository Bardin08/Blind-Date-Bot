using System;
using System.Linq;
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
    class NextDateCommand : IBotCommand
    {
        public static event Delegates.DateFoundHandler DateFound;

        public string Name => "/next_date";

        public async Task Execute(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as BaseTransactionModel;

            logger.LogDebug("Next date command was initiated by {username}({userid})",
                            currentTransaction.Message.From.Username,
                            currentTransaction.Message.From.Id);

            if (TransactionsContainer.DateForUserExists(currentTransaction.RecipientId))
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.YouHaveAnActiveDate);
                return;
            }

            await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.DateSearchText);

            var user = await db.Set<UserModel>().FirstOrDefaultAsync(u => u.TelegramId == currentTransaction.RecipientId);

            if (user == null)
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.InternalErrorUserNotFound);
                return;
            }

            user.IsVisible = true;
            db.Update(user);
            await db.SaveChangesAsync();

            Random rnd = new();

            var possibleInterlocutors = await db.Set<UserModel>().Where(u => u.IsFree == true
                                                           && user.InterlocutorGender == u.Gender
                                                           && user.Gender == u.InterlocutorGender
                                                           && u.Id != user.Id
                                                           && u.IsVisible).ToListAsync();

            var interlocutor = possibleInterlocutors[rnd.Next(0, possibleInterlocutors.Count == 0 
                ? possibleInterlocutors.Count 
                : possibleInterlocutors.Count - 1)];
            if (interlocutor == null)
            {
                return;
            }

            var dateModel = new DateModel(Guid.NewGuid().ToString())
            {
                FirstUser = user,
                SecondUser = interlocutor,
                IsActive = true
            };
                        
            user.IsFree = false;
            db.Update(user);

            interlocutor.IsFree = false;
            db.Update(interlocutor);

            await db.Set<DateModel>().AddAsync(dateModel);
            await db.SaveChangesAsync();

            logger.LogDebug("New data started. Members: {firstuser}, {seconduser}", user, interlocutor);

            await botClient.SendTextMessageAsync(user.TelegramId, Messages.DateHasBegan);
            await botClient.SendTextMessageAsync(interlocutor.TelegramId, Messages.DateHasBegan);

            DateFound?.Invoke(new DateTransactionModel(dateModel));
        }
    }
}
