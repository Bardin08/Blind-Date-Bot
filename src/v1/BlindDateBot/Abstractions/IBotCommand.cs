using System.Threading.Tasks;

using BlindDateBot.Data.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace BlindDateBot.Abstractions
{
    public interface IBotCommand
    {
        public string Name { get; }

        public Task Execute(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db);
    }
}
