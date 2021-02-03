using System.Collections.Generic;

using BlindDateBot.Behavior.DateStages;
using BlindDateBot.Domain.Models;
using BlindDateBot.Interfaces;

namespace BlindDateBot.Models
{
    public class DateTransactionModel : TransactionBaseModel
    {
        public DateTransactionModel(int recepientId) : base(recepientId)
        {
            State = new DateSearchInitiated();
            TransactionType = Enums.TransactionType.DataMessaging;
        }

        public IDateTransactionState State { get; set; }
        public List<MessageModel> Messages { get; set; }
    }
}
