using BlindDateBot.Abstractions;
using BlindDateBot.Behavior.RegistrationStages;
using BlindDateBot.Domain.Models;

using Telegram.Bot.Types;

namespace BlindDateBot.Models
{
    public class RegistrationTransactionModel : BaseTransactionModel
    {
        public RegistrationTransactionModel(Message message, string username, string firstname)
            : base(message)
        {
            UserFirstName = firstname;

            TransactionType = Enums.TransactionType.Registration;

            User = new UserModel
            {
                TelegramId = RecipientId,
                Username = username,
                IsFree = true
            };

            TransactionState = new RegistrationInitiated();
        }

        public string UserFirstName { get; set; }

        public UserModel User { get; set; }
        
        public ITransactionState TransactionState { get; set; }
    }
}
