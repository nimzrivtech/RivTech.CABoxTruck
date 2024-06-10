using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Services.Submission.Applicant;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Premiums
{
    public interface IRiskPremiumTransactionService
    {
        /// <summary>
        /// If from submission, no need to set oldRiskDetailId but if from Endorsement, Cancellation, or Reinstatement 
        /// then set the oldRiskDetailId.
        /// </summary>
        Task CalculateAsync(Guid newRiskDetailId, Guid? oldRiskDetailId = null, bool fromReinstatement = false);
        Task CalculateAfterReinstatementAsync(RiskDetail canceledRiskDetail);
        Task CalculateServiceFee(Guid riskDetailId, EndorsementData currentData, EndorsementData previousData);
    }

    public class RiskPremiumTransactionService : IRiskPremiumTransactionService
    {
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;
        private readonly IRiskPremiumTransactionFactory _riskPremiumTransactionFactory;
        private readonly IRiskVehiclePremiumTransactionRepository _riskVehiclePremiumTransactionRepository;
        private readonly IRiskPolicyPremiumTransactionRepository _riskPolicyPremiumTransactionRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IBindingService _bindingService;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly INameAndAddressService _nameAndAddressService;
        private readonly IBillingService _billingService;
        private readonly IBindingRepository _bindingRepository;

        public RiskPremiumTransactionService(
            IRiskRepository riskRepository,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository,
            IRiskPremiumTransactionFactory riskPremiumTransactionFactory,
            IRiskVehiclePremiumTransactionRepository riskVehiclePremiumTransactionRepository,
            IRiskPolicyPremiumTransactionRepository riskPolicyPremiumTransactionRepository,
            IRiskDetailRepository riskDetailRepository,
            IBindingService bindingService,
            IRiskCoverageRepository riskCoverageRepository,
            IVehicleService vehicleService,
            IVehicleRepository vehicleRepository,
            INameAndAddressService nameAndAddressService,
            IBillingService billingService,
            IBindingRepository bindingRepository
        )
        {
            _riskRepository = riskRepository;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
            _riskPremiumTransactionFactory = riskPremiumTransactionFactory;
            _riskVehiclePremiumTransactionRepository = riskVehiclePremiumTransactionRepository;
            _riskPolicyPremiumTransactionRepository = riskPolicyPremiumTransactionRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskCoverageRepository = riskCoverageRepository;
            _vehicleService = vehicleService;
            _vehicleRepository = vehicleRepository;
            _nameAndAddressService = nameAndAddressService;
            _billingService = billingService;
            _bindingRepository = bindingRepository;
        }

        public async Task CalculateAsync(Guid newRiskDetailId, Guid? oldRiskDetailId = null, bool fromReinstatement = false)
        {
            await CalculateVehicleTransactionsAsync(newRiskDetailId, oldRiskDetailId, fromReinstatement);
            await CalculateGLTransactionsAsync(newRiskDetailId, oldRiskDetailId, fromReinstatement);
            await CalculateTITransactionsAsync(newRiskDetailId, oldRiskDetailId, fromReinstatement);
            await CalculateNOTransactionsAsync(newRiskDetailId, oldRiskDetailId, fromReinstatement);
        }

        private async Task CalculateVehicleTransactionsAsync(Guid newRiskDetailId, Guid? oldRiskDetailId = null, bool fromReinstatement = false)
        {
            if (fromReinstatement == true && oldRiskDetailId is null)
                throw new ArgumentNullException(nameof(oldRiskDetailId));

            IEnumerable<RiskVehiclePremiumCollection> newRiskVehiclePremiumsList = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(newRiskDetailId);
            IEnumerable<RiskVehiclePremiumTransaction> oldRiskVehicleTransactionList = oldRiskDetailId is null ? null
                : await _riskVehiclePremiumTransactionRepository.GetAsync(oldRiskDetailId.Value, fromReinstatement);

            var existingTransactions = await _riskVehiclePremiumTransactionRepository.GetAsync(newRiskDetailId);
            foreach (var transaction in existingTransactions)
            {
                await _riskVehiclePremiumTransactionRepository.DeleteAsync(transaction);
            }

            var transactions = _riskPremiumTransactionFactory.CreateVehiclePremiumTransactions(newRiskDetailId, newRiskVehiclePremiumsList, oldRiskVehicleTransactionList);

            foreach (var transaction in transactions)
            {
                transaction.CalculatePremiumChange();

                var existingTransaction = await _riskVehiclePremiumTransactionRepository.GetAsync(newRiskDetailId, transaction.VehicleMainId);
                if (existingTransaction != null)
                {
                    await _riskVehiclePremiumTransactionRepository.DeleteAsync(existingTransaction);
                }
                await _riskVehiclePremiumTransactionRepository.CreateAsync(transaction);
            }
        }

        private async Task CalculateGLTransactionsAsync(Guid newRiskDetailId, Guid? oldRiskDetailId = null, bool fromReinstatement = false)
        {
            if (fromReinstatement == true && oldRiskDetailId is null)
                throw new ArgumentNullException(nameof(oldRiskDetailId));

            RiskPolicyPremiumCollection newRiskGLPremiums = await _riskPolicyPremiumRepository
                .GetByRiskDetailAsync(newRiskDetailId, Premium.TYPE_POLICY_GL);
            RiskPolicyPremiumTransaction previousTransaction = oldRiskDetailId is null
                ? null : await _riskPolicyPremiumTransactionRepository.GetAsync(oldRiskDetailId.Value, Premium.TYPE_POLICY_GL, fromReinstatement);

            var transaction = _riskPremiumTransactionFactory.CreatePolicyPremiumTransactions(newRiskDetailId, Premium.TYPE_POLICY_GL, newRiskGLPremiums, previousTransaction);
            transaction.CalculatePremiumChange();

            var existingTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(newRiskDetailId, Premium.TYPE_POLICY_GL);
            if (existingTransaction != null)
                await _riskPolicyPremiumTransactionRepository.DeleteAsync(existingTransaction);

            await _riskPolicyPremiumTransactionRepository.CreateAsync(transaction);

            var newRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(newRiskDetailId);

            if (newRiskDetail.RiskCoverages != null)
            {
                foreach (var item in newRiskDetail.RiskCoverages)
                {
                    if (item.RiskCoveragePremium != null)
                    {
                        item.RiskCoveragePremium.ProratedGeneralLiabilityPremium = transaction.CurrentPremium;
                    }
                }
            }
            _riskDetailRepository.Update(newRiskDetail);

            if (oldRiskDetailId != null)
            {
                var oldRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(oldRiskDetailId.Value);

                if (oldRiskDetail.RiskCoverages != null)
                {
                    foreach (var item in newRiskDetail.RiskCoverages)
                    {
                        if (item.RiskCoveragePremium != null)
                        {
                            item.RiskCoveragePremium.ProratedGeneralLiabilityPremium = transaction.CurrentPremium;
                        }
                    }
                }

                _riskDetailRepository.Update(oldRiskDetail);
            }

            await _riskDetailRepository.SaveChangesAsync();
        }

        private async Task CalculateTITransactionsAsync(Guid newRiskDetailId, Guid? oldRiskDetailId = null, bool fromReinstatement = false)
        {
            if (fromReinstatement == true && oldRiskDetailId is null)
                throw new ArgumentNullException(nameof(oldRiskDetailId));

            RiskPolicyPremiumCollection newRiskTIPremiums = await _riskPolicyPremiumRepository
                    .GetByRiskDetailAsync(newRiskDetailId, Premium.TYPE_POLICY_TI);

            if (newRiskTIPremiums != null && newRiskTIPremiums.RiskPremiums.Count > 0)
            {
                RiskPolicyPremiumTransaction previousTransaction = oldRiskDetailId is null
                    ? null : await _riskPolicyPremiumTransactionRepository.GetAsync(oldRiskDetailId.Value, Premium.TYPE_POLICY_TI, fromReinstatement);

                var transaction = _riskPremiumTransactionFactory.CreatePolicyPremiumTransactions(newRiskDetailId, Premium.TYPE_POLICY_TI, newRiskTIPremiums, previousTransaction);
                transaction.CalculatePremiumChange();

                var existingTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(newRiskDetailId, Premium.TYPE_POLICY_TI);
                if (existingTransaction != null)
                    await _riskPolicyPremiumTransactionRepository.DeleteAsync(existingTransaction);

                await _riskPolicyPremiumTransactionRepository.CreateAsync(transaction);

                var newRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(newRiskDetailId);

                if (newRiskDetail.RiskCoverages != null)
                {
                    foreach (var item in newRiskDetail.RiskCoverages)
                    {
                        if (item.RiskCoveragePremium != null)
                        {
                            item.RiskCoveragePremium.ProratedTrailerInterchangePremiumAL = transaction.CurrentPremium;
                        }
                    }
                }
                _riskDetailRepository.Update(newRiskDetail);

                if (oldRiskDetailId != null)
                {
                    var oldRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(oldRiskDetailId.Value);

                    if (oldRiskDetail.RiskCoverages != null)
                    {
                        foreach (var item in newRiskDetail.RiskCoverages)
                        {
                            if (item.RiskCoveragePremium != null)
                            {
                                item.RiskCoveragePremium.ProratedTrailerInterchangePremiumAL = transaction.CurrentPremium;
                            }
                        }
                    }

                    _riskDetailRepository.Update(oldRiskDetail);
                }

                await _riskDetailRepository.SaveChangesAsync();
            }
        }

        private async Task CalculateNOTransactionsAsync(Guid newRiskDetailId, Guid? oldRiskDetailId = null, bool fromReinstatement = false)
        {
            if (fromReinstatement == true && oldRiskDetailId is null)
                throw new ArgumentNullException(nameof(oldRiskDetailId));

            RiskPolicyPremiumCollection newRiskNOPremiums = await _riskPolicyPremiumRepository
                .GetByRiskDetailAsync(newRiskDetailId, Premium.TYPE_POLICY_NO);

            if (newRiskNOPremiums != null && newRiskNOPremiums.RiskPremiums.Count > 0)
            {
                RiskPolicyPremiumTransaction previousTransaction = oldRiskDetailId is null
                    ? null : await _riskPolicyPremiumTransactionRepository.GetAsync(oldRiskDetailId.Value, Premium.TYPE_POLICY_NO, fromReinstatement);

                var transaction = _riskPremiumTransactionFactory.CreatePolicyPremiumTransactions(newRiskDetailId, Premium.TYPE_POLICY_NO, newRiskNOPremiums, previousTransaction);
                transaction.CalculatePremiumChange();

                var existingTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(newRiskDetailId, Premium.TYPE_POLICY_NO);
                if (existingTransaction != null)
                    await _riskPolicyPremiumTransactionRepository.DeleteAsync(existingTransaction);

                await _riskPolicyPremiumTransactionRepository.CreateAsync(transaction);

                var newRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(newRiskDetailId);

                if (newRiskDetail.RiskCoverages != null)
                {
                    foreach (var item in newRiskDetail.RiskCoverages)
                    {
                        if (item.RiskCoveragePremium != null)
                        {
                            item.RiskCoveragePremium.ProratedNonOwnedTrailerPremium = transaction.CurrentPremium;
                        }
                    }
                }
                _riskDetailRepository.Update(newRiskDetail);

                if (oldRiskDetailId != null)
                {
                    var oldRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(oldRiskDetailId.Value);

                    if (oldRiskDetail.RiskCoverages != null)
                    {
                        foreach (var item in newRiskDetail.RiskCoverages)
                        {
                            if (item.RiskCoveragePremium != null)
                            {
                                item.RiskCoveragePremium.ProratedNonOwnedTrailerPremium = transaction.CurrentPremium;
                            }
                        }
                    }

                    _riskDetailRepository.Update(oldRiskDetail);
                }

                await _riskDetailRepository.SaveChangesAsync();
            }
        }

        public async Task CalculateAfterReinstatementAsync(RiskDetail canceledRiskDetail)
        {
            #region Vehicle Transactions
            var reinstatedVehiclePremiumsList = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(canceledRiskDetail.Id, fromReinstatement: true);
            var canceledVehiclePremiumTransactionsList = await _riskVehiclePremiumTransactionRepository.GetAsync(canceledRiskDetail.Id);

            // Clear any existing vehicle premium transaction fromReinstatement 
            // before adding new transactions.
            var existingVehicleTransactions = await _riskVehiclePremiumTransactionRepository.GetAsync(canceledRiskDetail.Id, fromReinstatement: true);
            foreach (var transaction in existingVehicleTransactions)
            {
                await _riskVehiclePremiumTransactionRepository.DeleteAsync(transaction);
            }

            var transactions = _riskPremiumTransactionFactory.CreateVehiclePremiumTransactions(
                canceledRiskDetail.Id,
                reinstatedVehiclePremiumsList,
                canceledVehiclePremiumTransactionsList);

            foreach (var transaction in transactions)
            {
                transaction.CalculatePremiumChange();
                transaction.TagAsFromReinstatement();
                await _riskVehiclePremiumTransactionRepository.CreateAsync(transaction);
            }

            #endregion

            #region GL Transaction

            var reinstatedGLPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_GL, fromReinstatement: true);
            var canceledGLTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_GL);

            // Clear any existing vehicle premium transaction fromReinstatement 
            // before adding new transactions.
            var existingReinstatementGLTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_GL, fromReinstatement: true);
            if (existingReinstatementGLTransaction != null)
            {
                await _riskPolicyPremiumTransactionRepository.DeleteAsync(existingReinstatementGLTransaction);
            }

            var newReinstatementGLTransactions = _riskPremiumTransactionFactory.CreatePolicyPremiumTransactions(
                canceledRiskDetail.Id,
                Premium.TYPE_POLICY_GL,
                reinstatedGLPremiums,
                canceledGLTransaction);

            newReinstatementGLTransactions.CalculatePremiumChange();
            newReinstatementGLTransactions.TagAsFromReinstatement();
            await _riskPolicyPremiumTransactionRepository.CreateAsync(newReinstatementGLTransactions);

            #endregion

            #region TI Transaction

            var reinstatedTIPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_TI, fromReinstatement: true);
            var canceledTITransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_TI);

            // Clear any existing vehicle premium transaction fromReinstatement 
            // before adding new transactions.
            var existingReinstatementTITransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_TI, fromReinstatement: true);
            if (existingReinstatementTITransaction != null)
            {
                await _riskPolicyPremiumTransactionRepository.DeleteAsync(existingReinstatementTITransaction);
            }

            var newReinstatementTITransactions = _riskPremiumTransactionFactory.CreatePolicyPremiumTransactions(
                canceledRiskDetail.Id,
                Premium.TYPE_POLICY_TI,
                reinstatedTIPremiums,
                canceledTITransaction);

            newReinstatementTITransactions.CalculatePremiumChange();
            newReinstatementTITransactions.TagAsFromReinstatement();
            await _riskPolicyPremiumTransactionRepository.CreateAsync(newReinstatementTITransactions);

            #endregion

            #region NO Transaction

            var reinstatedNOPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_NO, fromReinstatement: true);
            var canceledNOTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_NO);

            // Clear any existing vehicle premium transaction fromReinstatement 
            // before adding new transactions.
            var existingReinstatementNOTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_NO, fromReinstatement: true);
            if (existingReinstatementNOTransaction != null)
            {
                await _riskPolicyPremiumTransactionRepository.DeleteAsync(existingReinstatementNOTransaction);
            }

            var newReinstatementNOTransactions = _riskPremiumTransactionFactory.CreatePolicyPremiumTransactions(
                canceledRiskDetail.Id,
                Premium.TYPE_POLICY_NO,
                reinstatedNOPremiums,
                canceledNOTransaction);

            newReinstatementNOTransactions.CalculatePremiumChange();
            newReinstatementNOTransactions.TagAsFromReinstatement();
            await _riskPolicyPremiumTransactionRepository.CreateAsync(newReinstatementNOTransactions);

            #endregion
        }

        public async Task CalculateServiceFee(Guid riskDetailId, EndorsementData currentData, EndorsementData previousData)
        {
            var currentRiskDetail = currentData.RiskDetail;
            var previousRiskDetail = previousData.RiskDetail;
            var riskCoverages = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var riskCoverage = riskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();
            var riskCoveragePremium = riskCoverage.RiskCoveragePremium;

            var currentAutoLiabilityUnits = 0;
            var currentPhysicalDamageUnits = 0;
            var previousAutoLiabilityUnits = 0;
            var previousPhysicalDamageUnits = 0;

            RiskCoverage previousRiskCoverage = currentRiskDetail.RiskCoverages?.FirstOrDefault(x => x.OptionNumber == binding.BindOptionId);

            var allPreviousVehicles = (await _vehicleService.GetByRiskDetailIdIncludeAsync(previousRiskDetail.Id, true))?
                .OrderBy(vehicle => vehicle.VIN).ToList();
            allPreviousVehicles = allPreviousVehicles.Where(x => ((x.Options ?? "").Split(',')).Contains(previousRiskCoverage.OptionNumber.ToString()) == true && x.IsValidated == true).ToList();

            previousAutoLiabilityUnits = allPreviousVehicles.Count;

            foreach (var allPreviousVehicle in allPreviousVehicles)
            {
                if (allPreviousVehicle.HasCompFtLimit() || allPreviousVehicle.HasCollisionLimit())
                {
                    previousPhysicalDamageUnits++;
                }
            }

            RiskCoverage currentRiskCoverage = currentRiskDetail.RiskCoverages?.FirstOrDefault(x => x.OptionNumber == binding.BindOptionId);

            var allCurrentVehicles = await _vehicleService.GetByRiskDetailIdIncludeAsync(currentRiskDetail.Id, withDeleted: true);

            allCurrentVehicles = allCurrentVehicles.Where(x => ((x.Options ?? "").Split(',')).Contains(currentRiskCoverage.OptionNumber.ToString()) == true && x.IsValidated == true).ToList();

            currentAutoLiabilityUnits = allCurrentVehicles.Count;

            var riskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == currentRiskDetail.RiskId && x.IsProcessed == true);
            riskDetails = riskDetails.OrderByDescending(x => x.CreatedDate);
            var riskDetailIds = riskDetails.Select(x => x.Id);

            foreach (var currentVehicle in allCurrentVehicles)
            {
                var hasPreviousCompFTLimitOrCollisionLimit = false;

                foreach (var id in riskDetailIds)
                {
                    var riskDetailVehicle = await _vehicleRepository.GetByRiskDetailAndMainIdAsync(id, currentVehicle.MainId);
                    if (riskDetailVehicle is null)
                        break;

                    hasPreviousCompFTLimitOrCollisionLimit = riskDetailVehicle.HasCompFtLimit() || riskDetailVehicle.HasCollisionLimit();

                    if (hasPreviousCompFTLimitOrCollisionLimit)
                        break;
                }

                if ((currentVehicle.HasCompFtLimit() || currentVehicle.HasCollisionLimit()) && !hasPreviousCompFTLimitOrCollisionLimit)
                {
                    currentPhysicalDamageUnits++;
                }
            }

            var nameAddress = await _nameAndAddressService.GetNameAndAddress(currentRiskDetail.Id);
            var feeDetails = _billingService.GetFeeDetailsByState(nameAddress?.State);
            decimal RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.MultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.MultiplePDServiceFee)?.Amount ?? 0;

            decimal RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMultiplePDServiceFee)?.Amount ?? 0;

            decimal RISK_MANAGEMENT_FEE_FLAT_AL_MID_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMidtermALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_FLAT_PD_MID_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMidtermPDServiceFee)?.Amount ?? 0;

            var billingFees = await _billingService.GetBillingFeesAsync(currentRiskDetail.RiskId);
            decimal? alServiceFees = 0;
            decimal? pdServiceFees = 0;

            if (billingFees.Any())
            {
                alServiceFees = billingFees.Where(a => a.Description == "Service Fee - AL")?.Sum(a => a.Amount) ?? 0;
                pdServiceFees = billingFees.Where(a => a.Description == "Service Fee - PD")?.Sum(a => a.Amount) ?? 0;

                if (currentAutoLiabilityUnits > previousAutoLiabilityUnits)
                {
                    if (RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT > 0)
                        alServiceFees += RISK_MANAGEMENT_FEE_FLAT_AL_MID_UNIT;
                    else if (RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT > 0)
                        alServiceFees += (RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT * (currentAutoLiabilityUnits - previousAutoLiabilityUnits));
                }

                if (currentPhysicalDamageUnits > 0)
                {
                    if (RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT > 0)
                        pdServiceFees += RISK_MANAGEMENT_FEE_FLAT_PD_MID_UNIT;
                    else if (RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT > 0)
                        pdServiceFees += (RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT * currentPhysicalDamageUnits);
                }

                riskCoveragePremium.RiskMgrFeeAL = alServiceFees;
                riskCoveragePremium.RiskMgrFeePD = pdServiceFees;
                riskCoveragePremium.Fees = alServiceFees + pdServiceFees;
            }

            riskCoverage.RiskCoveragePremium = riskCoveragePremium;
            _riskCoverageRepository.Update(riskCoverage);
            await _riskCoverageRepository.SaveChangesAsync();

            var saveCurrentRiskCoverages = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(currentRiskDetail.Id);
            var saveRiskCoverage = saveCurrentRiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();
            var saveRiskCoveragePremium = saveRiskCoverage.RiskCoveragePremium;

            if (billingFees.Any())
            {
                saveRiskCoveragePremium.RiskMgrFeeAL = alServiceFees;
                saveRiskCoveragePremium.RiskMgrFeePD = pdServiceFees;
                saveRiskCoveragePremium.Fees = alServiceFees + pdServiceFees;
            }

            saveRiskCoverage.RiskCoveragePremium = saveRiskCoveragePremium;
            _riskCoverageRepository.Update(saveRiskCoverage);
            await _riskCoverageRepository.SaveChangesAsync();
        }
    }
}
