namespace BlindDateBot.Models
{
    public class CommandTransactionModel : TransactionBaseModel
    {
        public CommandTransactionModel(int recepientId) : base(recepientId)
        {
            TransactionType = Enums.TransactionType.Command;
        }
    }
}
