using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Strategies
{
    public class FeedbackProcessStrategy : ITransactionProcessStrategy
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            await (transaction as Models.FeedbackTransactionModel).TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
        }
    }
}
