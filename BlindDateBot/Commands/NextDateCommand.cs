using System;
using System.Linq;
using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Domain.Models;
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
            logger.LogDebug("Next date command was initiated by {username}({userid})", message.From.Username, message.From.Id);

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

            user.IsVisible = true;
            db.Update(user);
            await db.SaveChangesAsync();

            Random rnd = new();

            var possibleInterlocutors = await db.Users.Where(u => u.IsFree == true
                                                           && user.InterlocutorGender == u.Gender
                                                           && user.Gender == u.InterlocutorGender
                                                           && u.Id != user.Id
                                                           && u.IsVisible).ToListAsync();

            var interlocutor = possibleInterlocutors[rnd.Next(0, possibleInterlocutors.Count - 1)];
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

            logger.LogDebug("New data started. Members: {firstuser}, {seconduser}", user, interlocutor);

            await botClient.SendTextMessageAsync(user.TelegramId, Messages.DateHasBegan);
            await botClient.SendTextMessageAsync(interlocutor.TelegramId, Messages.DateHasBegan);

            DateFound?.Invoke(new DateTransactionModel(dateModel));
        }
    }
}
