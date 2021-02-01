using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Interfaces
{
    public interface IBotCommand
    {
        public string Name { get; }

        public Task Execute(Message message, object transaction, ITelegramBotClient botClient);
    }
}
