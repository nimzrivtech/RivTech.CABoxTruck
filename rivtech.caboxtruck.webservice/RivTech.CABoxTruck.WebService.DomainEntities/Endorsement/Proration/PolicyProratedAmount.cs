using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration
{
    public class PolicyProratedAmount : ProratedAmount
    {
        public RiskPolicyPremiumCollection RiskPolicyPremiums { get; }
        public RiskPolicyPremium SubmissionRiskPolicyPremium { get; }

        public PolicyProratedAmount(
            RiskPolicyPremiumCollection riskPolicyPremiums,
            AmountInfo info,
            DateRange policyPeriod,
            RiskPolicyPremium submissionRiskPolicyPremium
        ) : base(info, policyPeriod)
        {
            RiskPolicyPremiums = riskPolicyPremiums;
            SubmissionRiskPolicyPremium = submissionRiskPolicyPremium;

            bool isMatchingPremiumType = (RiskPolicyPremiums.PremiumType == Premium.TYPE_POLICY_GL && info.Kind == AmountKind.GeneralLiability) ||
                                         (RiskPolicyPremiums.PremiumType == Premium.TYPE_POLICY_TI && info.Kind == AmountKind.TrailerInterchange) ||
                                         (RiskPolicyPremiums.PremiumType == Premium.TYPE_POLICY_NO && info.Kind == AmountKind.NonOwnedTrailer);
            if (!isMatchingPremiumType)
            {
                throw new ArgumentException(@$"Premium type of {nameof(riskPolicyPremiums)} '{riskPolicyPremiums.PremiumType}' 
                    does not match premium type of {nameof(info)} '{info.Kind}'");
            }

            UpdatePeriods();
        }

        private void UpdatePeriods()
        {
            foreach (var rvp in RiskPolicyPremiums.Reverse())
            {
                var covPrem = rvp.CoveragePremium;

                var period = new ProratedAmountPeriod(covPrem.Id,
                                covPrem.Premium.Annual,
                                covPrem.CoveragePeriod,
                                PolicyPeriod,
                                rvp.Equals(SubmissionRiskPolicyPremium));

                _periods.AddLast(period);
            }
        }


    }
}
