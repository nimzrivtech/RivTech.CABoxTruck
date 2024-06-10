using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public abstract class CoveragePremiumBase
    {
        public CoveragePremiumBase() { }

        public long Id { get; set; }
        public DateRange CoveragePeriod { get; protected set; }
        public Premium Premium { get; protected set; }

        /// <summary>
        /// Changes the CoveragePeriod with new expiration date 
        /// and recalculates the PremiumCoverages Prorated property 
        /// with the new CoveragePeriod and the PolicyPeriod.
        /// </summary>
        public void Expire(DateTime expirationDate, DateRange policyPeriod)
        {
            CoveragePeriod = CoveragePeriod.WithNewEnd(expirationDate);
            RecalculateProratedPremium(policyPeriod);
        }

        public abstract void RecalculateProratedPremium(DateRange policyPeriod);
        public abstract CoveragePremiumBase Clone();
    }
}