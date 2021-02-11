﻿using System.Linq;
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
    class EndDateCommand : IBotCommand
    {
        public static event Delegates.DateEndHandler DateEnd;

        public string Name => "/end_date";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            logger.LogDebug("End date command was initiated by {username}({userid})", message.From.Username, message.From.Id);

            var cuurentTransaction = transaction as CommandTransactionModel;

            var date = db.Dates
                .Include(u => u.FirstUser)
                .Include(u => u.SecondUser)
                .FirstOrDefault(d => (d.FirstUser.TelegramId == cuurentTransaction.RecipientId
                                  || d.SecondUser.TelegramId == cuurentTransaction.RecipientId)
                                  && d.IsActive == true);

            if (date is null)
            { 
                return; 
            }

            UserModel user = db.Users.Find(date.FirstUser.Id);
            user.IsVisible = false;
            user.IsFree = true;
            db.Update(user);

            user = db.Users.Find(date.SecondUser.Id);
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
