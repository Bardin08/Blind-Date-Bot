using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Domain.Models.Enums;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BlindDateBot.Behavior.RegistrationStages
{
    internal class GenderReceived : ITransactionState
    {
        public async Task ProcessTransaction(
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDbContext db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            ValidateInputAndUpdateModel(currentTransaction,
                                        botClient,
                                        logger,
                                        db);

            var sentMessage = await botClient.SendTextMessageAsync(currentTransaction.RecipientId,
                                                 Messages.SelectInterlocuterGender,
                                                 replyMarkup: CreateReplyKeyboard());

            currentTransaction.MessageIds.Add(sentMessage.MessageId);

            currentTransaction.TransactionState = new InterlocuterGenderReceived();
        }

        private static async void ValidateInputAndUpdateModel(
            RegistrationTransactionModel transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDbContext db)
        {
            if (transaction.Message?.Text == null || !int.TryParse(transaction.Message.Text, out int genderId))
            {
                await botClient.SendTextMessageAsync(transaction.RecipientId, Messages.SomethingWentWrong);

                transaction.TransactionState = new RegistrationInitiated();
                await transaction.TransactionState.ProcessTransaction(transaction, botClient, logger, db);
                return;
            }

            if (genderId == 0)
            {
                transaction.User.Gender = Gender.Male;
            }
            else if (genderId == 1)
            {
                transaction.User.Gender = Gender.Female;
            }
        }

        private static InlineKeyboardMarkup CreateReplyKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    CallbackData = "0",
                    Text = Messages.InterlocutorMale
                },
                new InlineKeyboardButton()
                {
                    CallbackData = "1",
                    Text = Messages.InterlocutorFemale
                }
            });
        }
    }
}
