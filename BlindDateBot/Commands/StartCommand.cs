using System.Threading.Tasks;

using BlindDateBot.Interfaces;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class StartCommand : IBotCommand
    {
        public static event Delegates.RegistrationInitiatedHandler RegistrationInitiated;

        public string Name => "/start";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient)
        {
            var registrationTransaction = new Models.RegistrationTransactionModel(message.From.Id, message.From.Username, message.From.FirstName);
            RegistrationInitiated.Invoke(registrationTransaction);
            return;
        }
    }
}
