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
    public class GenderReceived : IRegistrationTransactionState
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
                currentTransaction.User.Gender = Gender.Male;
            }
            else if (genderId == 1)
            {
                currentTransaction.User.Gender = Gender.Female;
            }

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    CallbackData = "0",
                    Text = Messages.Male
                },
                new InlineKeyboardButton()
                {
                    CallbackData = "1",
                    Text = Messages.Female
                }

            });

            await botClient.SendTextMessageAsync(currentTransaction.RecepientId, Messages.SelectInterlocuterGender, replyMarkup: keyboard);

            currentTransaction.TransactionState = new InterlocuterGenderReceived();
        }
    }
}
