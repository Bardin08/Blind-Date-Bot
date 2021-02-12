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
    internal class ReportCommand : IBotCommand
    {
        public static event ReportHandler ReportInitiated;

        public string Name => "/report";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            var currentDate = TransactionsContainer.GetDateTransactionByRecipientId(message.From.Id);
            
            if (currentDate == null)
            {
                await botClient.SendTextMessageAsync(message.From.Id, Messages.NoActiveDate);
                return;
            }

            ReportTransactionModel transactionModel = new(message.From.Id)
            {
                UserWithComplaint = currentDate.Date.FirstUser.TelegramId != message.From.Id ? currentDate.Date.FirstUser : currentDate.Date.SecondUser
            };

            ReportInitiated?.Invoke(transactionModel);
        }
    }
}
