using System;
using System.Collections.Generic;

using BlindDateBot.Models.Enums;

namespace BlindDateBot.Models
{
    public class TransactionBaseModel
    {
        public TransactionBaseModel(int recepientId)
        {
            TransactionId = Guid.NewGuid().ToString();
            RecipientId = recepientId;
            IsComplete = false;
    
            MessageIds = new();
        }

        public bool IsComplete { get; set; }
        public string TransactionId { get; set; }
        public int RecipientId { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<int> MessageIds { get; set; }
    }
}
