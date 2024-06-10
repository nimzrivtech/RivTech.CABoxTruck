using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.ExternalData.Context
{
    /// <summary>
    /// The Database Context <see cref="DbContext"/> class.
    /// </summary>
    public partial class AppDbExternalContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbExternalContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public AppDbExternalContext(DbContextOptions<AppDbExternalContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbExternalContext).Assembly);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                return null;
            }

            _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);

            return _currentTransaction;
        }

        public async Task<int> CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
        {
            int result;
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction != _currentTransaction)
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            }

            try
            {
                result = await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await RollbackTransaction(cancellationToken);
                if (ex is DbUpdateConcurrencyException)
                {
                    List<string> metaDataNames = new List<string>();
                    var exception = ex as DbUpdateConcurrencyException;
                    foreach (var entry in exception.Entries)
                    {
                        metaDataNames.Add(entry.Metadata.Name);
                    }
                    var names = string.Join(", ", metaDataNames);
                    throw new DbUpdateConcurrencyException(
                        $"Concurrency Error: Data has been updated. Transaction Id: {transaction.TransactionId}. Meta data: {names}");
                }
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }

            return result;
        }

        public async Task RollbackTransaction(CancellationToken cancellationToken = default)
        {
            try
            {
                await _currentTransaction?.RollbackAsync(cancellationToken);
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity<object>> Entry<T>(BaseEntity<object> entity) where T : class
        {
            throw new NotImplementedException();
        }

        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public DbSet<EmailQueue> EmailQueues { get; set; }
        public DbSet<EmailQueueArchive> EmailQueueArchives { get; set; }
    }
}
