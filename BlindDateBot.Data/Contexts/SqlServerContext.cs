using BlindDateBot.Data.Configurations;

using Microsoft.EntityFrameworkCore;

namespace BlindDateBot.Data.Contexts
{
    public class SqlServerContext : BaseContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
        }
    }
}
