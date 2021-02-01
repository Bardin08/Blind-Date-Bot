using System.Text;
using System.Threading.Tasks;

using BlindDateBot.Data.Interfaces;
using BlindDateBot.Domain.Models.Enums;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BlindDateBot.Behavior.RegistrationStages
{
    public class InterlocuterGenderReceived : IRegistrationTransactionState
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, IDatabase db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            if (message?.Text == null || !int.TryParse(message.Text, out int genderId))
            {
                await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.SomethingWentWrong);

                currentTransaction.TransactionState = new RegistrationInitiated();
                await currentTransaction.TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
                return;
            }

            if (genderId == 0)
            {
                currentTransaction.User.InterlocutorGender = Gender.Male;
            }
            else if (genderId == 1)
            {
                currentTransaction.User.InterlocutorGender = Gender.Female;
            }

            var sb = new StringBuilder();

            var user = currentTransaction.User;

            sb.Append(string.Format(Messages.ConfirmData, user.Gender.ToString(), user.InterlocutorGender.ToString()));

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    Text = "Да",
                    CallbackData = "1"
                },
                new InlineKeyboardButton()
                {
                    Text = "Нет",
                    CallbackData = "0"
                }
            });

            await botClient.SendTextMessageAsync(currentTransaction.RecepientId, sb.ToString(), replyMarkup: keyboard);

            currentTransaction.TransactionState = new ConfirmationReceived();
        }
    }
}
