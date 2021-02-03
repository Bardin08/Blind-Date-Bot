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
            RecepientId = recepientId;
            IsComplete = false;
    
            MessageIds = new();
        }

        public bool IsComplete { get; set; }
        public string TransactionId { get; set; }
        public int RecepientId { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<int> MessageIds { get; set; }
    }
}
