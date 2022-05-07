using System.Collections.Generic;

namespace BlindDateBot.Domain.Models
{
    public class DateModel
    {
        public DateModel(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public UserModel FirstUser { get; set; }
        public UserModel SecondUser { get; set; }
        public bool IsActive { get; set; }
    }
}
