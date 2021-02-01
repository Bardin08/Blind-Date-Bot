using BlindDateBot.Options;

using Telegram.Bot;

namespace BlindDateBot.Interfaces
{
    public interface IBlindDateBotClient
    {
        public TelegramBotClient BotClient { get; set; }

        public BlindDateBotOptions Options { get; set; }
    }
}
