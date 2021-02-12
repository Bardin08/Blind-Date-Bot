using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Delegates;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace BlindDateBot.Commands
{
    internal class ReportCommand : IBotCommand
    {
        public static event ReportHandler ReportInitiated;

        public string Name => "/report";

        public async Task Execute(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as BaseTransactionModel;
            var currentDate = TransactionsContainer.GetDateTransactionByRecipientId(currentTransaction.Message.From.Id);
            
            if (currentDate == null)
            {
                await botClient.SendTextMessageAsync(currentTransaction.Message.From.Id, Messages.NoActiveDate);
                return;
            }

            ReportTransactionModel transactionModel = new(currentTransaction.Message)
            {
                UserWithComplaint = currentDate.Date.FirstUser.TelegramId != currentTransaction.Message.From.Id ? currentDate.Date.FirstUser : currentDate.Date.SecondUser
            };

            ReportInitiated?.Invoke(transactionModel);
        }
    }
}
