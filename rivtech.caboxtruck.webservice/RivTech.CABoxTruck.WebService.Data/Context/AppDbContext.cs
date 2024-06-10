using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Drivers;
using RivTech.CABoxTruck.WebService.Data.Entity.Endorsements;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.ProcessQueuing;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.Context
{
    /// <summary>
    /// The Database Context <see cref="DbContext"/> class.
    /// </summary>
    public partial class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
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

        public DbSet<Address> Address { get; set; }
        public DbSet<AdditionalInterestType> AdditionalInterestType { get; set; }
        public DbSet<AppLog> AppLog { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Entity.Entity> Entity { get; set; }
        public DbSet<EntityAddress> EntityAddress { get; set; }
        public DbSet<EntityContact> EntityContact { get; set; }
        public DbSet<EntitySubsidiary> EntitySubsidiary { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompany { get; set; }
        public DbSet<Insured> Insured { get; set; }
        public DbSet<RiskDetail> RiskDetail { get; set; }
        public DbSet<RiskAdditionalInterest> RiskAdditionalInterest { get; set; }
        public DbSet<RiskResponse> RiskResponse { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Validation> Validation { get; set; }
        public DbSet<QuestionValidation> QuestionValidation { get; set; }
        public DbSet<RiskCoverage> RiskCoverage { get; set; }
        public DbSet<RiskHistory> RiskHistory { get; set; }
        public DbSet<CoveragePlan> CoveragePlan { get; set; }
        public DbSet<CoveredSymbol> CoveredSymbol { get; set; }
        public DbSet<DriverTrainingOption> DriverTrainingOptions { get; set; }
        public DbSet<BackgroundCheckOption> BackgroundCheckOptions { get; set; }
        public DbSet<MvrPullingFrequencyOption> MvrPullingFrequencyOptions { get; set; }
        public DbSet<RiskSpecificSafetyDevice> RiskSpecificSafetyDevice { get; set; }
        public DbSet<ChameleonIssuesOption> ChameleonIssuesOptions { get; set; }
        public DbSet<SaferRatingOption> SaferRatingOptions { get; set; }
        public DbSet<TrafficLightRatingOption> TrafficLightRatingOptions { get; set; }
        public DbSet<RiskStatus> RiskStatus { get; set; }
        public DbSet<VehiclePremiumRatingFactor> VehiclePremiumRatingFactor { get; set; }
        public DbSet<DriverPremiumRatingFactor> DriverPremiumRatingFactor { get; set; }
        public DbSet<RiskCoveragePremium> RiskCoveragePremium { get; set; }

        public DbSet<Driver> Driver { get; set; }
        public DbSet<DriverHeader> DriverHeader { get; set; }

        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<VehiclePremium> VehiclePremium { get; set; }
        public DbSet<BrokerInfo> BrokerInfo { get; set; }

        public DbSet<LvLimits> LvLimits { get; set; }
        public DbSet<RiskNotes> RiskNotes { get; set; }
        public DbSet<NoteCategoryOption> NoteCategoryOption { get; set; }

        //public DbSet<PricingOption> PricingOptions { get; set; }
        public DbSet<BindOption> BindOptions { get; set; }
        public DbSet<BindStatus> BindStatus { get; set; }
        public DbSet<Subjectivities> Subjectivities { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Symbol> Symbols { get; set; }

        public DbSet<CreditedOffice> CreditedOffices { get; set; }
        public DbSet<BindRequirementsOption> BindRequirementsOption { get; set; }

        public DbSet<QuoteConditionsOption> QuoteConditionsOption { get; set; }

        public DbSet<PaymentTypes> PaymentTypes { get; set; }
        
        public DbSet<NonTaxableFeeData> NonTaxableFees { get; set; }
        public DbSet<FeeKindData> FeeKinds { get; set; }
        public DbSet<TaxDetailsData> TaxDetails { get; set; }
        public DbSet<StateTaxData> StateTax { get; set; }
        public DbSet<FileUploadDocument> FileUploadDocument { get; set; }
        public DbSet<FeeDetails> FeeDetails { get; set; }
        public DbSet<MainUse> MainUse { get; set; }
        public DbSet<RiskDetailDriver> RiskDetailDriver { get; set; }
        public DbSet<RiskDetailVehicle> RiskDetailVehicle { get; set; }
        public DbSet<EndorsementPendingChange> EndorsementPendingChanges { get; set; }
        public DbSet<PolicyHistory> PolicyHistory  { get; set; }
        public DbSet<CancellationBreakdown> CancellationBreakdown  { get; set; }
        public DbSet<ReinstatementBreakdown> ReinstatementBreakdown  { get; set; }
        public DbSet<RiskPolicyContact> RiskPolicyContact { get; set; }
        public DbSet<FTPDocument> FTPDocument { get; set; }
        public DbSet<FTPDocumentTemporary> FTPDocumentTemporary { get; set; }
        public DbSet<FTPDocumentFilesCount> FTPDocumentFilesCount { get; set; }
        public DbSet<EndorsementChanges> EndorsementChanges { get; set; }

        public DbSet<RiskVehiclePremium> RiskVehiclePremium { get; set; }
        public DbSet<RiskPolicyPremium> RiskPolicyPremium { get; set; }
        public DbSet<VehicleCoveragePremium> RiskVehiclePremiumCoverage { get; set; }
        public DbSet<PolicyCoveragePremium> RiskPolicyPremiumCoverage { get; set; }
        public DbSet<RiskVehiclePremiumTransaction> RiskVehiclePremiumTransaction { get; set; }
        public DbSet<RiskPolicyPremiumTransaction> RiskPolicyPremiumTransaction { get; set; }
        public DbSet<EndorsementProcessStatusLog> EndorsementProcessStatusLog { get; set; }
        public DbSet<EndorsementProcessQueue> EndorsementProcessQueue { get; set; }
        public DbSet<SymbolTenResponse> SymbolTenResponse { get; set; }

        public DbSet<LvDataset> LvDataset { get; set; }
        public DbSet<RiskCoveragePremiumAvg> RiskCoveragePremiumAvg { get; set; }
    }
}
