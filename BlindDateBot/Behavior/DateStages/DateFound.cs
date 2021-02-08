using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BlindDateBot.Data.Contexts;
using BlindDateBot.Domain.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace BlindDateBot.Behavior.DateStages
{
    public class DateFound : Interfaces.IDateTransactionState
    {
        public async Task ProcessTransaction(Message message, object transaction, ITelegramBotClient botClient, ILogger logger, SqlServerContext db)
        {
            var currentTransaction = transaction as Models.DateTransactionModel;

            List<UserModel> users = new()
            {
                currentTransaction.Date.FirstUser,
                currentTransaction.Date.SecondUser,
            };

            var messageModel = new MessageModel()
            {
                From = users.Where(u => u.TelegramId == message.From.Id).First(),
                To = users.Where(u => u.TelegramId != message.From.Id).First(),
                Text = message.Text
            };

            await botClient.SendTextMessageAsync(messageModel.To.TelegramId, messageModel.Text);
        }
    }
}
