using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Strategies
{
    public class RegistrationProcessStrategy : ITransactionProcessStrategy
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            await (transaction as RegistrationTransactionModel).TransactionState.ProcessTransaction(transaction, botClient, logger, db);
        }
    }
}
