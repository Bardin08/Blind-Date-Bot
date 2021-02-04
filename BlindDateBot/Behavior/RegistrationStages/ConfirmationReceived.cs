using System.Threading.Tasks;
using System.Linq;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Data.Repositories;
using BlindDateBot.Domain.Models;
using BlindDateBot.Interfaces;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;

namespace BlindDateBot.Behavior.RegistrationStages
{
    internal class ConfirmationReceived : IRegistrationTransactionState
    {
        public async Task ProcessTransaction(
            Message message,
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            SqlServerContext db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            await ValidateInputAndUpdateUserModel(message,
                                                  currentTransaction,
                                                  botClient,
                                                  logger,
                                                  db);

            AddUserToDb(currentTransaction.User, db);

            var sentMessage = await botClient.SendTextMessageAsync(currentTransaction.RecepientId,
                                                 Messages.RegistrationComplete,
                                                 replyMarkup: GenerateReplyMarkup());

            currentTransaction.MessageIds.Add(sentMessage.MessageId);

            currentTransaction.TransactionState = new InterlocuterGenderReceived();
            currentTransaction.IsComplete = true;
        }

        private static async void AddUserToDb(UserModel user, SqlServerContext db)
        {
             var existingUser = await db.Users
                .FirstOrDefaultAsync(u => u.TelegramId == user.TelegramId);

            if (existingUser != null)
            {
                user.Id = existingUser.Id;

                existingUser.Gender = user.Gender;
                existingUser.InterlocutorGender = user.InterlocutorGender;
                existingUser.IsFree = user.IsFree;

                db.Update(existingUser);
            }
            else
            {
                await db.AddAsync(user);
            }

            await db.SaveChangesAsync();
        }

        private static async Task ValidateInputAndUpdateUserModel(
            Message message,
            RegistrationTransactionModel transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            SqlServerContext db)
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
