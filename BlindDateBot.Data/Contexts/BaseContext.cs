using BlindDateBot.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace BlindDateBot.Data.Contexts
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) 
            : base(options) { }

        public DbSet<UserModel> Users { get; set; }
    }
}
