﻿using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Interfaces
{
    public interface IBotCommand
    {
        public string Name { get; }

        public Task Execute(Message messages, object transaction, ITelegramBotClient botClient);
    }
}
