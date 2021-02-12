﻿using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Contexts;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Strategies
{
    public class DefaultProcessStrategy : ITransactionProcessStrategy
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            await botClient.SendTextMessageAsync(message.From.Id, "Default handler works!");
        }
    }
}
