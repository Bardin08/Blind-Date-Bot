using BlindDateBot.Behavior.FeedbackStates;
using BlindDateBot.Interfaces;

namespace BlindDateBot.Models
{
    public class FeedbackTransactionModel : TransactionBaseModel
    {
        public FeedbackTransactionModel(int recepientId)
            : base(recepientId)
        {
            TransactionType = Enums.TransactionType.Feedback;

            TransactionState = new FeedbackInitiated();
        }

        public IFeedbackTransactionState TransactionState { get; set; }
    }
}
