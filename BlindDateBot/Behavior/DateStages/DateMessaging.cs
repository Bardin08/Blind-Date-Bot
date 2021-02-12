using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlindDateBot.Abstractions;
using BlindDateBot.Data.Abstractions;
using BlindDateBot.Domain.Models;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace BlindDateBot.Behavior.DateStages
{
    public class DateMessaging : ITransactionState
    {
        public async Task ProcessTransaction(object transaction, ITelegramBotClient botClient, ILogger logger, IDbContext db)
        {
            var currentTransaction = transaction as Models.DateTransactionModel;

            List<UserModel> users = new()
            {
                currentTransaction.Date.FirstUser,
                currentTransaction.Date.SecondUser,
            };

            var messageObject = new 
            {
                From = users.Where(u => u.TelegramId == currentTransaction.Message.From.Id).First(),
                To = users.Where(u => u.TelegramId != currentTransaction.Message.From.Id).First(),
            };

            ForwardMessage(currentTransaction.Message, messageObject.To, botClient);
        }

        private static async void ForwardMessage(Message message, UserModel recipient, ITelegramBotClient botClient)
        {
            _ = message.Type switch
            {
                MessageType.Unknown => await botClient.SendTextMessageAsync(recipient.TelegramId, "Bad message!"),
                MessageType.Text => await botClient.SendTextMessageAsync(recipient.TelegramId, message.Text),
                MessageType.Photo => await botClient.SendPhotoAsync(recipient.TelegramId, new InputOnlineFile(message.Photo[0].FileId)),
                MessageType.Audio => await botClient.SendAudioAsync(recipient.TelegramId, new InputOnlineFile(message.Audio.FileId)),
                MessageType.Video => await botClient.SendVideoAsync(recipient.TelegramId, new InputOnlineFile(message.Video.FileId)),
                MessageType.Voice => await botClient.SendVoiceAsync(recipient.TelegramId, new InputOnlineFile(message.Voice.FileId)),
                MessageType.Document => await botClient.SendDocumentAsync(recipient.TelegramId, new InputOnlineFile(message.Document.FileId)),
                MessageType.Sticker => await botClient.SendStickerAsync(recipient.TelegramId, new InputOnlineFile(message.Sticker.FileId)),
                MessageType.Contact => await botClient.SendContactAsync(recipient.TelegramId, message.Contact.PhoneNumber, message.Contact.FirstName, message.Contact.LastName), 
                MessageType.VideoNote => await botClient.SendVideoNoteAsync(recipient.TelegramId, message.VideoNote.FileId),
            };
        }
    }
}
