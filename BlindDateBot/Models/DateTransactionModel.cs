using BlindDateBot.Abstractions;
using BlindDateBot.Behavior.DateStages;
using BlindDateBot.Domain.Models;

using Telegram.Bot.Types;

namespace BlindDateBot.Models
{
    public class DateTransactionModel : BaseTransactionModel
    {
        public const int RecipientIdForDateTransaction = -1;

        public DateTransactionModel(DateModel dateModel)
            : base(new Message() { From = new User() { Id = RecipientIdForDateTransaction } })
        {
            RecipientId = RecipientIdForDateTransaction;
            State = new DateMessaging();
            TransactionType = Enums.TransactionType.DateMessaging;

            TransactionId = dateModel.Id;
            Date = dateModel;
        }

        public DateModel Date { get; set; }
        
        public ITransactionState State { get; set; }
    }
}
