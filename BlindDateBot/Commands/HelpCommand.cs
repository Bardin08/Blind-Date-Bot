using System.Threading.Tasks;

using BlindDateBot.Interfaces;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class HelpCommand : IBotCommand
    {
        public string Name => "/help";

        public async Task Execute(Message message, object transaction, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.From.Id, "Help command");       
        }
    }
}
