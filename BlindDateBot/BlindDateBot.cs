using System;
using System.Threading;

using BlindDateBot.Interfaces;
using BlindDateBot.Models;
using BlindDateBot.Options;

using Telegram.Bot;
using Telegram.Bot.Args;

namespace BlindDateBot
{
    public class BlindDateBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BlindDateBotOptions _options;

        private IMessageProcessingStrategy _messageProcessor;

        public BlindDateBot(IBlindDateBotClient botClient)
        {
            _botClient = botClient.BotClient;
            _options = botClient.Options;

            _messageProcessor = new Strategies.CommandProcessStrategy();
        }

        public void Execute()
        {
            _botClient.OnMessage += MessageReceived;

            _botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private async void MessageReceived(object sender, MessageEventArgs e)
        {
            await _messageProcessor.ProcessTransaction(e.Message, new CommandTransaction(e.Message.From.Id), _botClient);
        }
    }
}
