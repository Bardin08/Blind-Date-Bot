using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Strategies
{
    public class DefaultProcessStrategy : ITransactionProcessStrategy
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            await botClient.SendTextMessageAsync((transaction as BaseTransactionModel).Message.From.Id, "Default handler works!");
        }
    }
}
