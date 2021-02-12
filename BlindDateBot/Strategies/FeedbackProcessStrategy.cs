using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Strategies
{
    public class FeedbackProcessStrategy : ITransactionProcessStrategy
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            await (transaction as Models.FeedbackTransactionModel).TransactionState.ProcessTransaction(transaction, botClient, logger, db);
        }
    }
}
