using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.ExternalData.Context;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public abstract class ExternalBaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : RivTech.CABoxTruck.WebService.ExternalData.Entity.BaseEntity<TKey>
    {
        protected readonly AppDbExternalContext _context;

        protected ExternalBaseRepository(AppDbExternalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _context.AddAsync(entity);
            return result.Entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().AnyAsync(expression);
        }

        public virtual async Task<TEntity> FindAsync(TKey key)
        {
            return await _context.FindAsync<TEntity>(key);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            var result = await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
            return result;
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] include)
        {
            return await SetInclude(include).FirstOrDefaultAsync(expression);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().AsNoTracking().Where(expression).ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] include)
        {
            return await SetInclude(include).AsNoTracking().Where(expression).ToListAsync();
        }

        public TEntity Update(TEntity entity)
        {
            return _context.Update(entity).Entity;
        }

        public virtual void UpdateRange(params object[] entities)
        {
            _context.UpdateRange(entities);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);
        }

        public virtual TEntity Remove(TEntity entity)
        {
            return _context.Remove(entity).Entity;
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.RemoveRange(entities);
        }

        public virtual EntityEntry Entry(object entity)
        {
            return _context.Entry(entity);
        }

        public virtual EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return _context.Entry<TEntity>(entity);
        }

        public virtual async Task<int> SaveChangesAsync(bool updateCreatedBy = true)
        {
            var enumerationEntries = _context.ChangeTracker.Entries()
                .Where(x => x.Entity is Enumeration<short> || x.Entity is Enumeration<string>);

            foreach (var enumerationEntry in enumerationEntries)
            {
                enumerationEntry.State = EntityState.Unchanged;
            }

            var userId = Services.GetCurrentUser();

            foreach (var entityEntry in _context.ChangeTracker.Entries().Where(x => x.Entity is ICreatedData && x.State == EntityState.Added))
            {
                var createdData = entityEntry.Entity as ICreatedData;
                var now = DateTime.Now;
                createdData.CreatedDate = createdData.CreatedDate == null || createdData.CreatedDate == DateTime.MinValue ? now : createdData.CreatedDate;

                if(updateCreatedBy)
                    createdData.CreatedBy = userId;
            }

            foreach (var entityEntry in _context.ChangeTracker.Entries().Where(x => x.Entity is IUpdatedData))
            {
                var createdData = entityEntry.Entity as IUpdatedData;
                var now = DateTime.Now;
                switch (entityEntry.State)
                {
                    case EntityState.Modified:
                        createdData.UpdatedDate = now;
                        break;
                    case EntityState.Added:
                        createdData.UpdatedDate = now;
                        break;
                }
            }

            var result = 0;
            _context.ChangeTracker.DetectChanges();
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.BeginTransactionAsync();
                result = await _context.CommitTransactionAsync(transaction);
            });
            return result;
        }

        protected IQueryable<T> SetInclude<T>(params Expression<Func<T, object>>[] includedProperties) where T : RivTech.CABoxTruck.WebService.ExternalData.Entity.BaseEntity<TKey>
        {
            IQueryable<T> query = _context.Set<T>();
            if (includedProperties != null)
            {
                return includedProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return query;
        }

        public void Attach(TEntity entity)
        {
            _context.Attach(entity);
        }

        public void Detach(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void Detach(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
        }
    }
}
