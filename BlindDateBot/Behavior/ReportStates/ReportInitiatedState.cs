using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BlindDateBot.Behavior.ReportStates
{
    public class ReportInitiatedState : IReportTransactionState
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            var currentTransaction = transaction as ReportTransactionModel;

            await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.ReportInitiated, ParseMode.Markdown);

            currentTransaction.TransactionState = new ReportReasonMessageReceived();
        }
    }
}
