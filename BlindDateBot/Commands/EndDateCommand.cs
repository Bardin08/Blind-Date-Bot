﻿using System.Linq;
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
    class EndDateCommand : IBotCommand
    {
        public static event Delegates.DateEndHandler DateEnd;

        public string Name => "/end_date";

        public async Task Execute(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var cuurentTransaction = transaction as CommandTransactionModel;

            var date = db.Set<DateModel>()
                .Include(u => u.FirstUser)
                .Include(u => u.SecondUser)
                .FirstOrDefault(d => (d.FirstUser.TelegramId == cuurentTransaction.RecipientId
                                  || d.SecondUser.TelegramId == cuurentTransaction.RecipientId)
                                  && d.IsActive == true);

            if (date == null)
            { 
                return; 
            }

            UserModel user = db.Set<UserModel>().Find(date.FirstUser.Id);
            user.IsVisible = false;
            user.IsFree = true;
            db.Update(user);

            user = db.Set<UserModel>().Find(date.SecondUser.Id);
            user.IsVisible = false;
            user.IsFree = true;
            db.Update(user);

            date.IsActive = false;
            db.Update(date);
            await db.SaveChangesAsync();

            await botClient.SendTextMessageAsync(date.FirstUser.TelegramId, Messages.DateEnd);
            await botClient.SendTextMessageAsync(date.SecondUser.TelegramId, Messages.DateEnd);

            DateEnd?.Invoke(date.Id);
        }
    }
}
