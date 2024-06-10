using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public class EndorsementDataTransactionCalculator : IEndorsementDataTransactionCalculator
    {
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskVehiclePremiumTransactionRepository _riskVehiclePremiumTransactionRepository;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;
        private readonly IRiskPolicyPremiumTransactionRepository _riskPolicyPremiumTransactionRepository;
        private readonly IRiskPremiumTransactionFactory _riskPremiumTransactionFactory;

        public EndorsementDataTransactionCalculator(
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskVehiclePremiumTransactionRepository riskVehiclePremiumTransactionRepository,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository,
            IRiskPolicyPremiumTransactionRepository riskPolicyPremiumTransactionRepository,
            IRiskPremiumTransactionFactory riskPremiumTransactionFactory
        )
        {
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskVehiclePremiumTransactionRepository = riskVehiclePremiumTransactionRepository;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
            _riskPolicyPremiumTransactionRepository = riskPolicyPremiumTransactionRepository;
            _riskPremiumTransactionFactory = riskPremiumTransactionFactory;
        }

        public async Task CalculateChangesAsync(IEndorsementData newEndorsementData, IEndorsementData oldEndorsementData)
        {
            await CalculateVehiclePremiumTransactions(newEndorsementData, oldEndorsementData);
            //await CalculatePolicyPremiumTransactions(newEndorsementData, oldEndorsementData);
        }

        private async Task CalculatePolicyPremiumTransactions(IEndorsementData newEndorsementData, IEndorsementData oldEndorsementData)
        {
            var oldGLRiskPolicyPremiums = await _riskPolicyPremiumRepository.GetByTypeAsync(newEndorsementData.RiskDetailId, Premium.TYPE_POLICY_GL);
            var newGLRiskPolciyPremiums = await _riskPolicyPremiumRepository.GetByTypeAsync(oldEndorsementData.RiskDetailId, Premium.TYPE_POLICY_GL);

            var isGLNewlyAdded = !oldGLRiskPolicyPremiums.Any();

            // Create premium transaction.
            RiskPolicyPremiumTransaction newPremiumTransaction;
            if (isGLNewlyAdded)
            {
                newPremiumTransaction = RiskPolicyPremiumTransaction.FromNewPremium(newGLRiskPolciyPremiums);
            }
            else
            {
                newPremiumTransaction = RiskPolicyPremiumTransaction.FromExistingPremium(newGLRiskPolciyPremiums, oldGLRiskPolicyPremiums);
            }

            // Save transaction.
            await _riskPolicyPremiumTransactionRepository.CreateAsync(newPremiumTransaction);
        }

        private async Task CalculateVehiclePremiumTransactions(IEndorsementData newEndorsementData, IEndorsementData oldEndorsementData)
        {
            // Calculate vehicle premium changes
            var oldRiskVehiclePremiumsList = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(oldEndorsementData.RiskDetailId);
            var newRiskVehiclePremiumsList = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(newEndorsementData.RiskDetailId);
            var transactions = _riskPremiumTransactionFactory.CreateVehiclePremiumTransactions(newRiskVehiclePremiumsList, oldRiskVehiclePremiumsList);

            foreach(var transaction in transactions)
            {
                transaction.CalculatePremiumChange();
                await _riskVehiclePremiumTransactionRepository.CreateAsync(transaction);
            }
        }
    }
}
