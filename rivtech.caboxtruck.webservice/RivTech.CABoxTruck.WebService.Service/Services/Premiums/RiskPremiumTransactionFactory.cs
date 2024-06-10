using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Premiums
{
    public interface IRiskPremiumTransactionFactory
    {
        IEnumerable<RiskVehiclePremiumTransaction> CreateVehiclePremiumTransactions(Guid newRiskDetailId, IEnumerable<RiskVehiclePremiumCollection> newRiskVehiclePremiumsList, IEnumerable<RiskVehiclePremiumTransaction> oldRiskVehicleTransactionList);
        RiskPolicyPremiumTransaction CreatePolicyPremiumTransactions(Guid newRiskDetailId, string premiumType, RiskPolicyPremiumCollection newRiskGLPremiums, RiskPolicyPremiumTransaction oldRiskGLTransaction);
    }

    public class RiskPremiumTransactionFactory : IRiskPremiumTransactionFactory
    {
        public RiskPolicyPremiumTransaction CreatePolicyPremiumTransactions(
            Guid newRiskDetailId,
            string premiumType,
            RiskPolicyPremiumCollection newRiskPolicyPremiums,
            RiskPolicyPremiumTransaction previousTransaction
        ) {
            // Calculate vehicle premium changes
            // Get the matching vehicle premium from previous endorsement.
            var transaction = new RiskPolicyPremiumTransaction(
                newRiskDetailId,
                premiumType,
                newRiskPolicyPremiums, 
                previousTransaction);
            transaction.CalculatePremiumChange();
            return transaction;
        }

        public IEnumerable<RiskVehiclePremiumTransaction> CreateVehiclePremiumTransactions(
            Guid riskDetailId,
            IEnumerable<RiskVehiclePremiumCollection> newRiskVehiclePremiumsList,
            IEnumerable<RiskVehiclePremiumTransaction> oldRiskVehiclePremiumTransactionList = null
        )
        {
            var premiumTransactions = new List<RiskVehiclePremiumTransaction>();
            oldRiskVehiclePremiumTransactionList ??= new List<RiskVehiclePremiumTransaction>();

            // Calculate vehicle premium changes
            foreach (RiskVehiclePremiumCollection riskVehiclePremiums in newRiskVehiclePremiumsList)
            {
                // Get the matching vehicle premium from previous endorsement.
                var previousRiskVehiclePremiumTransaction = oldRiskVehiclePremiumTransactionList.FirstOrDefault(x => x.VehicleMainId.Equals(riskVehiclePremiums.VehicleMainId));
                var transaction = new RiskVehiclePremiumTransaction(riskDetailId, riskVehiclePremiums.VehicleMainId, riskVehiclePremiums, previousRiskVehiclePremiumTransaction);
                transaction.CalculatePremiumChange();
                premiumTransactions.Add(transaction);
            }

            return premiumTransactions;
        }
    }
}
