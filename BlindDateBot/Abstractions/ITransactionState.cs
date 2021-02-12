using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace BlindDateBot.Abstractions
{
    public interface ITransactionState
    {
        Task ProcessTransaction(Message message, object transaction);
    }
}
