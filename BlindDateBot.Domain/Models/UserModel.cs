using System;

using BlindDateBot.Domain.Models.Enums;

namespace BlindDateBot.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Gender Gender { get; set; }
        public Gender InterlocutorGender{ get; set; }
        public bool IsFree { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserModel model &&
                   Id == model.Id &&
                   Username == model.Username &&
                   Gender == model.Gender;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Username, Gender);
        }

        public override string ToString()
        {
            return $"User({GetHashCode()}): {Username}[({Id}),{(Gender == Gender.Male ? "Male" : "Female" )}]";
        }
    }
}
