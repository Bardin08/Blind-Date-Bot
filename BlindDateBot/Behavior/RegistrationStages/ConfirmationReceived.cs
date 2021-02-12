using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Domain.Models;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;
using BlindDateBot.Data.Abstractions;

namespace BlindDateBot.Behavior.RegistrationStages
{
    internal class ConfirmationReceived : ITransactionState
    {
        public async Task ProcessTransaction(
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDbContext db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            var isValid = await ValidateInputAndUpdateUserModel(currentTransaction,
                                                  botClient,
                                                  logger,
                                                  db);

            if (isValid)
            {
                AddUserToDb(currentTransaction.User, db);

                var sentMessage = await botClient.SendTextMessageAsync(currentTransaction.RecipientId,
                                                     Messages.RegistrationComplete,
                                                     replyMarkup: GenerateReplyMarkup());

                currentTransaction.MessageIds.Add(sentMessage.MessageId);

                currentTransaction.TransactionState = new InterlocuterGenderReceived();
                currentTransaction.IsComplete = true;
            }
        }

        private static async void AddUserToDb(UserModel user, IDbContext db)
        {
             var existingUser = await db.Set<UserModel>()
                .FirstOrDefaultAsync(u => u.TelegramId == user.TelegramId);

            if (existingUser != null)
            {
                user.Id = existingUser.Id;

                existingUser.Gender = user.Gender;
                existingUser.InterlocutorGender = user.InterlocutorGender;
                existingUser.IsFree = user.IsFree;

                db.Set<UserModel>().Update(existingUser);
            }
            else
            {
                await db.Set<UserModel>().AddAsync(user);
            }

            await db.SaveChangesAsync();
        }

        private static async Task<bool> ValidateInputAndUpdateUserModel(
            RegistrationTransactionModel transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDbContext db)
        {
            if (transaction.Message?.Text == null || !int.TryParse(transaction.Message.Text, out int reply))
            {
                await botClient.SendTextMessageAsync(transaction.RecipientId, Messages.SomethingWentWrong);

                transaction.TransactionState = new RegistrationInitiated();
                await transaction.TransactionState.ProcessTransaction(transaction, botClient, logger, db);
                return false;
            }

            bool isConfirmed = true;
            if (reply == 0)
            {
                isConfirmed = false;
            }

            if (!isConfirmed)
            {
                transaction.TransactionState = new RegistrationInitiated();
                await transaction.TransactionState.ProcessTransaction(transaction, botClient, logger, db);
                return false;
            }

            return true;
        }

        private static InlineKeyboardMarkup GenerateReplyMarkup()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    Text = Messages.FindDate,
                    CallbackData = "/next_date",
                }
           });
        }
    }
}
