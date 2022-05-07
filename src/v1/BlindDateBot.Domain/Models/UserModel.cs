using System;

using BlindDateBot.Domain.Models.Enums;

namespace BlindDateBot.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public int TelegramId { get; set; }
        public int ComplaintsAmount{ get; set; }
        public string Username { get; set; }
        public string BlockReason{ get; set; }
        public Gender Gender { get; set; }
        public Gender InterlocutorGender{ get; set; }
        public bool IsFree { get; set; }
        public bool IsVisible { get; set; }
        public bool IsBlocked { get; set; }

        public UserModel()
        {
            IsFree = true;
            IsVisible = false;
        }

        public override bool Equals(object obj)
        {
            return obj is UserModel model &&
                   TelegramId == model.TelegramId &&
                   Username == model.Username &&
                   Gender == model.Gender;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TelegramId, Username, Gender);
        }

        public override string ToString()
        {
            return $"User({GetHashCode()}): {Username}[({TelegramId}),{(Gender == Gender.Male ? "Male" : "Female" )}]";
        }
    }
}
