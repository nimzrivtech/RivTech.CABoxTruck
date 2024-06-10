using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public class RiskPolicyPremiumTransaction : RiskPremiumTransactionBase
    {
        private RiskPolicyPremiumTransaction() : base()  { } // For EF use

        public RiskPolicyPremiumTransaction(
            Guid riskDetailId,
            string premiumType,
            RiskPolicyPremiumCollection riskPolicyPremiums,
            RiskPolicyPremiumTransaction previousTransaction
        ) : base(riskDetailId)
        {
            PremiumType = premiumType;
            RiskPolicyPremiums = riskPolicyPremiums;
            PreviousTransaction = previousTransaction;
        }

        public string PremiumType { get; private set; }
        public RiskPolicyPremiumCollection RiskPolicyPremiums { get; private set; }
        public RiskPolicyPremiumTransaction PreviousTransaction { get; private set; }

        public override void CalculatePremiumChange()
        {
            CurrentPremium = RiskPolicyPremiums.TotalProratedPremium;
            PreviousPremium = PreviousTransaction?.CurrentPremium ?? 0m;

            base.CalculatePremiumChange();
        }
    }
}
