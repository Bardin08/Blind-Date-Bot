using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Delegates;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Commands
{
    public class FeedbackCommamd : IBotCommand
    {
        public static event FeedbackTransactionInitiaded FeedbackTransactionInitiated;

        public string Name => "/feedback";

        public async Task Execute(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as BaseTransactionModel;

            logger.LogDebug("Feedback command was initiated by {username}({userid})",
                            currentTransaction.Message.From.Username,
                            currentTransaction.Message.From.Id);

            FeedbackTransactionInitiated?.Invoke(new FeedbackTransactionModel(currentTransaction.Message));
            return;
        }
    }
}
