using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Strategies
{
    public class RegistrationProcessStrategy : Interfaces.IMessageProcessingStrategy
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            await (transaction as RegistrationTransactionModel).TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
        }
    }
}
