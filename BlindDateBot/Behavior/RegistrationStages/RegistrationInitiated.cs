using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BlindDateBot.Behavior.RegistrationStages
{
    public class RegistrationInitiated : ITransactionState
    {
        public async Task ProcessTransaction(
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            IDbContext db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            var sentMessage = await botClient.SendTextMessageAsync(currentTransaction.RecipientId,
                                                 string.Format(Messages.RegistrationInitMessage, currentTransaction.UserFirstName),
                                                 replyMarkup: CreateReplyKeyboard());

            currentTransaction.MessageIds.Add(sentMessage.MessageId);

            currentTransaction.TransactionState = new GenderReceived();
        }

        private static InlineKeyboardMarkup CreateReplyKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton()
                {
                    CallbackData = "0",
                    Text = Messages.IMale
                },
                new InlineKeyboardButton()
                {
                    CallbackData = "1",
                    Text = Messages.IFemale
                }
            });
        }
    }
}
