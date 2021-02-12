using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Strategies
{
    public class DateProcessStrategy : ITransactionProcessStrategy
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            if (message != null)
            {
                await (transaction as DateTransactionModel)?.State.ProcessTransaction(message, transaction, botClient, logger, db);
            }
        }
    }
}
