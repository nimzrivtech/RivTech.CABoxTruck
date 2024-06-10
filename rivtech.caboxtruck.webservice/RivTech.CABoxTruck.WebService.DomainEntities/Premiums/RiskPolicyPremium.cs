using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public class RiskPolicyPremium : RiskPremiumBase<PolicyCoveragePremium>
    {
        public string PremiumType { get; set; }

        public override object Clone()
        {
            return new RiskPolicyPremium
            {
                Id = Guid.Empty,
                RiskDetailId = RiskDetailId,
                PremiumType = PremiumType,
                CoveragePremium = CoveragePremium,
                FromReinstatement = FromReinstatement
            };
        }

        public override bool Equals(object obj)
        {
            var other = obj as RiskPolicyPremium;
            return other != null 
                && other.RiskDetailId == RiskDetailId
                && other.PremiumType == PremiumType
                && other.CoveragePremium.Equals(CoveragePremium);
        }

    }
}
