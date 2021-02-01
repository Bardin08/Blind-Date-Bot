namespace BlindDateBot.Models
{
    public class CommandTransaction : TransactionBase
    {
        public CommandTransaction(int transactionId)
        {
            IsComplete = false;
            TransactionType = "CommandTransaction";
            TransactionId = RecepientId = transactionId;

            MessageIds = new();
        }
    }
}
