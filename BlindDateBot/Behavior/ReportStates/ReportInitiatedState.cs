using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BlindDateBot.Behavior.ReportStates
{
    public class ReportInitiatedState : ITransactionState
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as ReportTransactionModel;

            await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.ReportInitiated, ParseMode.Markdown);

            currentTransaction.TransactionState = new ReportReasonMessageReceived();
        }
    }
}
