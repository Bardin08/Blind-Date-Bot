using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Interfaces
{
    public interface IMessageProcessingStrategy
    {
        Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient);
    }
}
