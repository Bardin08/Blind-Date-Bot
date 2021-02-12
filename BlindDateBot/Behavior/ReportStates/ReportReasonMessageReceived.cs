using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Domain.Models;
using BlindDateBot.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Behavior.ReportStates
{
    public class ReportReasonMessageReceived : IReportTransactionState
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            var currentTransaction = transaction as ReportTransactionModel;

            currentTransaction.ReportReason = message.Text;

            UserModel userWithComplaint = await db.Users.FirstOrDefaultAsync(u => u.Id == currentTransaction.UserWithComplaint.Id);

            if (userWithComplaint == null)
            {
                return;
            }

            if (userWithComplaint.ComplaintsAmount >= 2)
            {
                userWithComplaint.ComplaintsAmount++;
                userWithComplaint.IsBlocked = true;
                userWithComplaint.IsFree = false;
                userWithComplaint.IsVisible = false;
                userWithComplaint.BlockReason = currentTransaction.ReportReason;
                await botClient.SendTextMessageAsync(userWithComplaint.TelegramId, Messages.YourAccountBlocked);
            }
            else
            {
                userWithComplaint.ComplaintsAmount++;
                await botClient.SendTextMessageAsync(userWithComplaint.TelegramId, Messages.SomebodyComplainedAboutYou);
            }

            db.Update(userWithComplaint);
            await db.SaveChangesAsync();

            await botClient.SendTextMessageAsync(currentTransaction.RecipientId, Messages.YourReportSent);
            currentTransaction.IsComplete = true;

            await new Commands.EndDateCommand()
                .Execute(new Message() { Text = "/end_date", From = new User { Id = currentTransaction.RecipientId } },
                         new CommandTransactionModel(currentTransaction.RecipientId),
                         botClient,
                         logger,
                         db);
        }
    }
}
