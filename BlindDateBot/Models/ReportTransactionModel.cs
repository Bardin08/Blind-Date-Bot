using BlindDateBot.Behavior.ReportStates;
using BlindDateBot.Domain.Models;
using BlindDateBot.Interfaces;

namespace BlindDateBot.Models
{
    public class ReportTransactionModel : TransactionBaseModel
    {
        public ReportTransactionModel(int recepientId) 
            : base(recepientId)
        {
            TransactionType = Enums.TransactionType.Report;

            TransactionState = new ReportInitiatedState();
        }

        public IReportTransactionState TransactionState { get; set; }
        public string ReportReason { get; set; }
        public UserModel UserWithComplaint { get; set; }
    }
}
