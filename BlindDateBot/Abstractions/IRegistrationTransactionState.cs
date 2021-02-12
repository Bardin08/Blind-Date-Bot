using System.Threading.Tasks;
using BlindDateBot.Data.Contexts;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Abstractions
{
    public interface IRegistrationTransactionState
    {
        Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db);
    }
}
