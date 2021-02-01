using BlindDateBot.Interfaces;
using BlindDateBot.Options;

using Microsoft.Extensions.Configuration;

using Telegram.Bot;

namespace BlindDateBot
{
    public class BlindDataBotClient : IBlindDateBotClient
    {
        public TelegramBotClient BotClient { get; set; }

        public BlindDateBotOptions Options { get; set; }

        public BlindDataBotClient(IConfiguration configuration)
        {
            Options = new BlindDateBotOptions();

            configuration.GetSection("Bots:BlindDateBot")?.Bind(Options);

            BotClient = new TelegramBotClient(Options.BotToken);
        }
    }
}
