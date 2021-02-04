using BlindDateBot.Data.Configurations;

using Microsoft.EntityFrameworkCore;

namespace BlindDateBot.Data.Contexts
{
    public class SqlServerContext : BaseContext
    {
        private readonly string _connectionString;

        public SqlServerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
        }
    }
}
