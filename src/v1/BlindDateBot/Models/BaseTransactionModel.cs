using System;
using System.Collections.Generic;

using BlindDateBot.Models.Enums;

using Telegram.Bot.Types;

namespace BlindDateBot.Models
{
    public class BaseTransactionModel
    {
        public BaseTransactionModel(Message message)
        {
            TransactionId = Guid.NewGuid().ToString();
            RecipientId = message.From.Id;
            IsComplete = false;

            MessageIds = new();
        }

        public string TransactionId { get; set; }
        public int RecipientId { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<int> MessageIds { get; set; }
        public Message? Message { get; set; }
        public bool IsComplete { get; set; }
    }
}
