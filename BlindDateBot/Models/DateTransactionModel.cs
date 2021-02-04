using System.Collections.Generic;

using BlindDateBot.Behavior.DateStages;
using BlindDateBot.Domain.Models;
using BlindDateBot.Interfaces;

namespace BlindDateBot.Models
{
    public class DateTransactionModel : TransactionBaseModel
    {
        public DateTransactionModel(int recepientId, DateModel dateModel)
            : base(recepientId)
        {
            State = new DateFound();
            TransactionType = Enums.TransactionType.DateMessaging;

            Date = dateModel;
            Date.Id = TransactionId;
        }

        public DateModel Date { get; set; }
        
        public IDateTransactionState State { get; set; }
    }
}
