namespace BlindDateBot.Domain.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public UserModel From { get; set; }
        public UserModel To { get; set; }
    }
}