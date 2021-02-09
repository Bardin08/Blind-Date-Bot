using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Delegates;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class FeedbackCommamd : IBotCommand
    {
        public static event FeedbackTransactionInitiaded FeedbackTransactionInitiaded;

        public string Name => "/feedback";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            FeedbackTransactionInitiaded?.Invoke(new FeedbackTransactionModel(message.From.Id));
            return;
        }
    }
}
