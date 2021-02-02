using System.Collections.Generic;

using BlindDateBot.Models.Enums;

namespace BlindDateBot.Models
{
    public class TransactionBaseModel
    {
        public TransactionBaseModel(int transactionId)
        {
            TransactionId = RecepientId = transactionId;
            IsComplete = false;
    
            MessageIds = new();
        }

        public bool IsComplete { get; set; }
        public int TransactionId { get; set; }
        public int RecepientId { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<int> MessageIds { get; set; }
    }
}
