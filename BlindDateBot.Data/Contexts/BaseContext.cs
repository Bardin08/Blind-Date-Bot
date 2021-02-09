using BlindDateBot.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace BlindDateBot.Data.Contexts
{
    public class BaseContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<DateModel> Dates { get; set; }
    }
}
