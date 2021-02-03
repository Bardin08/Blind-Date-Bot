using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BlindDateBot.Behavior.RegistrationStages
{
    public class RegistrationInitiated : Interfaces.IRegistrationTransactionState
    {
        public async Task ProcessTransaction(
            Message message,
            object transaction,
            ITelegramBotClient botClient,
            ILogger logger,
            SqlServerContext db)
        {
            var currentTransaction = transaction as RegistrationTransactionModel;

            await botClient.SendTextMessageAsync(currentTransaction.RecepientId,
                                                 Messages.RegistrationInitMessage,
                                                 replyMarkup: CreateReplyKeyboard());

            currentTransaction.TransactionState = new GenderReceived();
        }

        private static InlineKeyboardMarkup CreateReplyKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
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
        }
    }
}
