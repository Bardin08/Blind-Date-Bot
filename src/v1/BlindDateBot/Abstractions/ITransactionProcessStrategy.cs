using System.Threading.Tasks;

using BlindDateBot.Data.Abstractions;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Abstractions
{
    public interface ITransactionProcessStrategy
    {
        Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db);
    }
}
