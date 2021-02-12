using System.Threading.Tasks;

using BlindDateBot.Data.Abstractions;
using BlindDateBot.Data.Configurations;
using BlindDateBot.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace BlindDateBot.Data.Contexts
{
    public class SqlServerContext : DbContext, IDbContext 
    {
        private readonly string _connectionString;

        public SqlServerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<DateModel> Dates { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        void IDbContext.Update<TEntity>(TEntity entity)
        {
            base.Update(entity);
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
