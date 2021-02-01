using System.Threading.Tasks;

using BlindDateBot.Interfaces;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Commands
{
    public class StartCommand : IBotCommand
    {
        public string Name { get => "/start"; }

        public async Task Execute(Message messages, object transaction, ITelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(messages.From.Id, messages.Text);
        }
    }
}
