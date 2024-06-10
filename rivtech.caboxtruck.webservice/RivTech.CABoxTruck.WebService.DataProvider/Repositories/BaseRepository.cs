using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Enum;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        protected readonly AppDbContext _context;

        protected BaseRepository(AppDbContext context)
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
            AddAppLog(userId);
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.BeginTransactionAsync();
                result = await _context.CommitTransactionAsync(transaction);
            });
            return result;
        }

        private void AddAppLog(long userId)
        {
            var auditEntries = new List<AppLogDTO>();
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.Entity is AppLog || entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var logEntry = new AppLogDTO(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId
                };
                auditEntries.Add(logEntry);
                foreach (var property in entry.Properties)
                {
                    var propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        logEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            logEntry.AuditType = AppLogType.Create;
                            logEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            logEntry.AuditType = AppLogType.Delete;
                            logEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                logEntry.ChangedColumns.Add(propertyName);
                                logEntry.AuditType = AppLogType.Update;
                                logEntry.OldValues[propertyName] = property.OriginalValue;
                                logEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                _context.AppLog.Add(auditEntry.ToAudit());
            }
        }

        protected IQueryable<T> SetInclude<T>(params Expression<Func<T, object>>[] includedProperties) where T : BaseEntity<TKey>
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
