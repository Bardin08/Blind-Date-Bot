using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Behavior.FeedbackStates
{
    public class FeedbackMessageReceived : Interfaces.IFeedbackTransactionState
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            await botClient.ForwardMessageAsync(new ChatId(Constants.FeedbackChannelId), message.From.Id, message.MessageId);

            (transaction as TransactionBaseModel).IsComplete = true;
            return;
        }
    }
}
