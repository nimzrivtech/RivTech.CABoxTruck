using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public class PolicyCoveragePremium : CoveragePremiumBase
    {
        public IEnumerable<RiskPolicyPremium> RiskPolicyPremiums { get; set; }

        private PolicyCoveragePremium() { }

        /// <summary>
        /// Factory method that creates new PolicyPremiumCoverage.
        /// </summary>
        public static PolicyCoveragePremium CreateNew(DateRange coveragePeriod, DateRange policyPeriod, Premium coveragePremium)
        {
            var coverage = new PolicyCoveragePremium()
            {
                CoveragePeriod = coveragePeriod,
                Premium = coveragePremium
            };
            coverage.RecalculateProratedPremium(policyPeriod);
            return coverage;
        }

        public override void RecalculateProratedPremium(DateRange policyPeriod)
        {
            Premium = Premium.Prorate(CoveragePeriod, policyPeriod);
        }

        public override CoveragePremiumBase Clone()
        {
            return new PolicyCoveragePremium()
            {
                CoveragePeriod = CoveragePeriod,
                Premium = Premium.Clone(),
            };
        }

        public override bool Equals(object obj)
        {
            var other = obj as PolicyCoveragePremium;
            return other.Premium.Equals(Premium)
                    && other.CoveragePeriod.Equals(CoveragePeriod);
        }
    }
}
