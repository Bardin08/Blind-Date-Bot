namespace BlindDateBot.Models
{
    public class CommandTransactionModel : TransactionBaseModel
    {
        public CommandTransactionModel(int transactionId) : base(transactionId)
        {
            TransactionType = "CommandTransaction";
        }
    }
}
