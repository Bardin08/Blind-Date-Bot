using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Delegates;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class FeedbackCommamd : IBotCommand
    {
        public static event FeedbackTransactionInitiaded FeedbackTransactionInitiated;

        public string Name => "/feedback";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            logger.LogDebug("Feedback command was initiated by {username}({userid})", message.From.Username, message.From.Id);

            FeedbackTransactionInitiated?.Invoke(new FeedbackTransactionModel(message.From.Id));
            return;
        }
    }
}
