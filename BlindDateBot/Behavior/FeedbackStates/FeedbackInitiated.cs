using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Behavior.FeedbackStates
{
    public class FeedbackInitiated : IFeedbackTransactionState
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            FeedbackTransactionModel currentTransaction = transaction as FeedbackTransactionModel;


            await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.FeedbackInitiated);

            currentTransaction.TransactionState = new FeedbackMessageReceived();
            return;
        }
    }
}
