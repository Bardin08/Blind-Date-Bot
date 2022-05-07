using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Data.Contexts;
using BlindDateBot.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class StartCommand : IBotCommand
    {
        public static event Delegates.RegistrationInitiatedHandler RegistrationInitiated;

        public string Name => "/start";

        public async Task Execute(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as CommandTransactionModel;

            logger.LogDebug("Start command was initiated by {username}({userid})",
                            currentTransaction.Message.From.Username,
                            currentTransaction.Message.From.Id);

            if (TransactionsContainer.DateForUserExists(currentTransaction.Message.From.Id))
            {
                await botClient.SendTextMessageAsync(currentTransaction.Message.From.Id, Messages.YouHaveAnActiveDate);
                return;
            }

            var registrationTransaction = new Models.RegistrationTransactionModel(currentTransaction.Message,
                                                                                  currentTransaction.Message.From.Username,
                                                                                  currentTransaction.Message.From.FirstName);
            RegistrationInitiated?.Invoke(registrationTransaction);
            return;
        }
    }
}
