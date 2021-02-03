using System.Threading.Tasks;

using BlindDateBot.Models;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    class NextDateCommand : Interfaces.IBotCommand
    {
        public static Delegates.DateInitiatedHandler DateInitiated;

        public string Name => "/next_date";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient)
        {
            DateInitiated?.Invoke(new DateTransactionModel(message.From.Id));
        }
    }
}
