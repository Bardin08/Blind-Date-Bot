using System.Collections.Generic;

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
        public string TransactionType { get; set; }
        public List<int> MessageIds { get; set; }
    }
}
