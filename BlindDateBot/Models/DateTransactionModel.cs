using BlindDateBot.Behavior.DateStages;
using BlindDateBot.Domain.Models;
using BlindDateBot.Interfaces;

namespace BlindDateBot.Models
{
    public class DateTransactionModel : TransactionBaseModel
    {
        public const int RecepientIdForDateTransaction = -1;

        public DateTransactionModel(DateModel dateModel)
            : base(RecepientIdForDateTransaction)
        {
            State = new DateFound();
            TransactionType = Enums.TransactionType.DateMessaging;

            TransactionId = dateModel.Id;
            Date = dateModel;
        }

        public DateModel Date { get; set; }
        
        public IDateTransactionState State { get; set; }
    }
}
