using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Behavior.FeedbackStates
{
    public class FeedbackMessageReceived : ITransactionState
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as FeedbackTransactionModel;

            await botClient.ForwardMessageAsync(new ChatId(Constants.FeedbackChannelId),
                                                currentTransaction.Message.From.Id,
                                                currentTransaction.Message.MessageId);

            (transaction as BaseTransactionModel).IsComplete = true;
            return;
        }
    }
}
