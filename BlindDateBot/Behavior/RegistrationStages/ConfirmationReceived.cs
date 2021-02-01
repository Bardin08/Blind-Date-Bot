using System.Text;
using System.Threading.Tasks;

using BlindDateBot.Data.Interfaces;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BlindDateBot.Behavior.RegistrationStages
{
    public class ConfirmationReceived : IRegistrationTransactionState
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, IDatabase db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            if (message?.Text == null || !int.TryParse(message.Text, out int reply))
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.SomethingWentWrong);

                currentTransaction.TransactionState = new RegistrationInitiated();
                await currentTransaction.TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
                return;
            }

            bool isConfirmed = true;
            if (reply == 0)
            {
                isConfirmed = false;
            }

            if (!isConfirmed)
            {
                currentTransaction.TransactionState = new RegistrationInitiated();
                await currentTransaction.TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
                return;
            }

           var keyboard = new InlineKeyboardMarkup(new[]
           {
                new InlineKeyboardButton()
                {
                    Text = Messages.FindDate,
                    CallbackData = "/next_date"
                }
           });

            await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.RegistrationComplete, replyMarkup: keyboard);

            currentTransaction.TransactionState = new InterlocuterGenderReceived();
            currentTransaction.IsComplete = true;
        }
    }
}
