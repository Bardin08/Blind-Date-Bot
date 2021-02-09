using System;
using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Domain.Models;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    class NextDateCommand : IBotCommand
    {
        public static event Delegates.DateFoundHandler DateFound;

        public string Name => "/next_date";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            var currentTransaction = transaction as TransactionBaseModel;

            if (TransactionsContainer.DateForUserExists(currentTransaction.RecipientId))
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.YouHaveAnActiveDate);
                return;
            }

            await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.DateSearchText);

            var user = await db.Users.FirstOrDefaultAsync(u => u.TelegramId == currentTransaction.RecipientId);
            if (user == null)
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.InternalErrorUserNotFound);
                return;
            }

                        var interlocutor = await db.Users.FirstOrDefaultAsync(u => u.IsFree == true
                                                                       && user.InterlocutorGender == u.Gender
                                                                       && user.Gender == u.InterlocutorGender
                                                                       && u.Id != user.Id);
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


            await db.Dates.AddAsync(dateModel);
            await db.SaveChangesAsync();

            await botClient.SendTextMessageAsync(user.TelegramId, Messages.DateHasBegan);
            await botClient.SendTextMessageAsync(interlocutor.TelegramId, Messages.DateHasBegan);

            DateFound?.Invoke(new DateTransactionModel(dateModel));
        }
    }
}
