using BlindDateBot.Abstractions;
using BlindDateBot.Behavior.ReportStates;
using BlindDateBot.Domain.Models;

using Telegram.Bot.Types;

namespace BlindDateBot.Models
{
    public class ReportTransactionModel : BaseTransactionModel
    {
        public ReportTransactionModel(Message message) 
            : base(message)
        {
            TransactionType = Enums.TransactionType.Report;

            TransactionState = new ReportInitiatedState();
        }

        public ITransactionState TransactionState { get; set; }
        public string ReportReason { get; set; }
        public UserModel UserWithComplaint { get; set; }
    }
}
