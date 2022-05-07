using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Behavior.FeedbackStates
{
    public class FeedbackInitiated : ITransactionState
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            FeedbackTransactionModel currentTransaction = transaction as FeedbackTransactionModel;


            await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.FeedbackInitiated);

            currentTransaction.TransactionState = new FeedbackMessageReceived();
            return;
        }
    }
}
