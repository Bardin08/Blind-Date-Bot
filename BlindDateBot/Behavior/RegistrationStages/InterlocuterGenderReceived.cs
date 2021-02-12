using System.Text;
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
    internal class InterlocuterGenderReceived : ITransactionState
    {
        public async Task ProcessTransaction(
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDbContext db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            ValidateInputAndUpdateUserModel(currentTransaction,
                                            botClient,
                                            logger,
                                            db);

            var sb = new StringBuilder();
            sb.Append(string.Format(Messages.ConfirmData,
                                    currentTransaction.User.Gender.ToString(),
                                    currentTransaction.User.InterlocutorGender.ToString()));

            var sentMessage = await botClient.SendTextMessageAsync(currentTransaction.RecipientId,
                                                 sb.ToString(),
                                                 replyMarkup: CreateReplyKeyboard());

            currentTransaction.MessageIds.Add(sentMessage.MessageId);
            
            currentTransaction.TransactionState = new ConfirmationReceived();
        }

        private static async void ValidateInputAndUpdateUserModel(
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
                transaction.User.InterlocutorGender = Gender.Male;
            }
            else if (genderId == 1)
            {
                transaction.User.InterlocutorGender = Gender.Female;
            }
        }
    
        private static InlineKeyboardMarkup CreateReplyKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    Text = Messages.Yes,
                    CallbackData = "1"
                },
                new InlineKeyboardButton()
                {
                    Text = Messages.No,
                    CallbackData = "0"
                }
            });
        }
    }
}
