using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IRepository<TEntity, TKey>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] include);
        Task<TEntity> FindAsync(TKey key);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] include);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
        TEntity Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void UpdateRange(params object[] entities);
        EntityEntry Entry(object entity);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(bool updateCreatedBy = true);
        void Attach(TEntity entity);
        void Detach(TEntity entity);
        void Detach(IEnumerable<TEntity> entities);
    }
}
