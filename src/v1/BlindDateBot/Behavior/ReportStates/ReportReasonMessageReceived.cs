using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Domain.Models;
using BlindDateBot.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Behavior.ReportStates
{
    public class ReportReasonMessageReceived : ITransactionState
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as ReportTransactionModel;

            currentTransaction.ReportReason = currentTransaction.Message.Text;

            UserModel userWithComplaint = await db.Set<UserModel>().FirstOrDefaultAsync(u => u.Id == currentTransaction.UserWithComplaint.Id);

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
                .Execute(
                 new CommandTransactionModel(currentTransaction.Message) 
                    { 
                        Message = new Message() 
                        { 
                            Text = "/end_date", 
                            From = new User 
                            { 
                                Id = currentTransaction.RecipientId 
                            } 
                        } 
                    },
                    botClient,
                    logger,
                    db);
        }
    }
}
