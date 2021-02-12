using BlindDateBot.Abstractions;
using BlindDateBot.Behavior.RegistrationStages;
using BlindDateBot.Domain.Models;

namespace BlindDateBot.Models
{
    public class RegistrationTransactionModel : TransactionBaseModel
    {
        public string UserFirstName { get; set; }

        public UserModel User { get; set; }
        
        public IRegistrationTransactionState TransactionState { get; set; }

        public RegistrationTransactionModel(int recepientId, string username, string firstname) 
            : base(recepientId)
        {
            UserFirstName = firstname;

            TransactionType = Enums.TransactionType.Registration;

            User = new UserModel
            {
                TelegramId = recepientId,
                Username = username,
                IsFree = true
            };
            
            TransactionState = new RegistrationInitiated();
        }
    }
}
