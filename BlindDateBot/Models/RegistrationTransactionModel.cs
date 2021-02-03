using BlindDateBot.Behavior.RegistrationStages;
using BlindDateBot.Domain.Models;
using BlindDateBot.Interfaces;

namespace BlindDateBot.Models
{
    public class RegistrationTransactionModel : TransactionBaseModel
    {
        public string UserFirstName { get; set; }

        public UserModel User { get; set; }
        
        public IRegistrationTransactionState TransactionState { get; set; }

        public RegistrationTransactionModel(int id, string username, string firstname) : base(id)
        {
            UserFirstName = firstname;

            TransactionId = RecepientId = id;

            TransactionType = Enums.TransactionType.Registration;

            User = new UserModel
            {
                TelegramId = id,
                Username = username,
                IsFree = true
            };
            
            TransactionState = new RegistrationInitiated();
        }
    }
}
