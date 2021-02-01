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
    internal class ConfirmationReceived : IRegistrationTransactionState
    {
        public async Task ProcessTransaction(
            Message message,
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDatabase db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            await ValidateInputAndUpdateUserModel(message,
                                                  currentTransaction,
                                                  botClient,
                                                  logger,
                                                  db);

            await botClient.SendTextMessageAsync(currentTransaction.RecepientId,
                                                 Messages.RegistrationComplete,
                                                 replyMarkup: GenerateReplyMarkup());

            currentTransaction.TransactionState = new InterlocuterGenderReceived();
            currentTransaction.IsComplete = true;
        }

        private static async Task ValidateInputAndUpdateUserModel(
            Message message,
            RegistrationTransactionModel transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDatabase db)
        {
            if (message?.Text == null || !int.TryParse(message.Text, out int reply))
            {
                await botClient.SendTextMessageAsync(transaction.RecepientId, Messages.SomethingWentWrong);

                transaction.TransactionState = new RegistrationInitiated();
                await transaction.TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
                return;
            }

            bool isConfirmed = true;
            if (reply == 0)
            {
                isConfirmed = false;
            }

            if (!isConfirmed)
            {
                transaction.TransactionState = new RegistrationInitiated();
                await transaction.TransactionState.ProcessTransaction(message, transaction, botClient, logger, db);
                return;
            }
        }

        private static InlineKeyboardMarkup GenerateReplyMarkup()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    Text = Messages.FindDate,
                    CallbackData = "/next_date"
                }
           });
        }
    }
}
