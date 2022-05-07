using BlindDateBot.Abstractions;
using BlindDateBot.Behavior.FeedbackStates;

using Telegram.Bot.Types;

namespace BlindDateBot.Models
{
    public class FeedbackTransactionModel : BaseTransactionModel
    {
        public FeedbackTransactionModel(Message message)
            : base(message)
        {
            TransactionType = Enums.TransactionType.Feedback;

            TransactionState = new FeedbackInitiated();
        }

        public ITransactionState TransactionState { get; set; }
    }
}
