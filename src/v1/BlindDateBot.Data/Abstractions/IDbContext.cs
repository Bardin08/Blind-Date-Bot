using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace BlindDateBot.Data.Abstractions
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();
        void Update<TEntity>(TEntity entity) where TEntity : class;
    }
}
