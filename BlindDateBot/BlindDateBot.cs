using System;
using System.Threading;

using BlindDateBot.Interfaces;
using BlindDateBot.Options;

using Telegram.Bot;
using Telegram.Bot.Args;

namespace BlindDateBot
{
    public class BlindDateBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BlindDateBotOptions _options;

        public BlindDateBot(IBlindDateBotClient botClient)
        {
            _botClient = botClient.BotClient;
            _options = botClient.Options;
        }

        public void Execute()
        {
            _botClient.OnMessage += MessageReceived;

            _botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private async void MessageReceived(object sender, MessageEventArgs e)
        {
            await _botClient.SendTextMessageAsync(e.Message.From.Id, e.Message.Text);
        }
    }
}
