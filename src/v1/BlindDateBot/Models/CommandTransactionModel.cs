using Telegram.Bot.Types;

namespace BlindDateBot.Models
{
    public class CommandTransactionModel : BaseTransactionModel
    {
        public CommandTransactionModel(Message message) 
            : base(message)
        {
            TransactionType = Enums.TransactionType.Command;
        }
    }
}
