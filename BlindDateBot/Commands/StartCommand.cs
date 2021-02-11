﻿using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class StartCommand : IBotCommand
    {
        public static event Delegates.RegistrationInitiatedHandler RegistrationInitiated;

        public string Name => "/start";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            logger.LogDebug("Start command was initiated by {username}({userid})", message.From.Username, message.From.Id);

            //if (await db.Users.FirstOrDefaultAsync(u => u.TelegramId == message.From.Id) != null)
            //{
            //    logger.LogDebug("User {username}({userid}) is already registered.");
            //    await botClient.SendTextMessageAsync(message.From.Id, Messages.AlreadyRegisteredUser);
            //    return;
            //}

            var registrationTransaction = new Models.RegistrationTransactionModel(message.From.Id, message.From.Username, message.From.FirstName);
            RegistrationInitiated?.Invoke(registrationTransaction);
            return;
        }
    }
}
