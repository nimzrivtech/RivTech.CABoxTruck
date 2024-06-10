using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{

    /// <summary>
    /// Each instance of this object represents the different annual premium in different effectivity periods of type CoveragePremiumBase.
    /// <para>
    /// For example:
    /// </para>
    /// <para>
    /// <para>
    ///     01/01/2020 - 03/31/2020 - Annual: 1000 Prorated: 243.84,
    /// </para>
    /// <para>
    ///     04/01/2020 - 12/31/2020 - Annual: 1200 Prorated: 900.82 
    /// </para>
    /// </para>
    /// </summary>
    public abstract class RiskPremiumBase<T> where T : CoveragePremiumBase
    {
        public RiskPremiumBase()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public T CoveragePremium { get; set; }
        public DateTime EffectiveDate => CoveragePremium.CoveragePeriod.Start.Date;
        public DateTime ExpirationDate => CoveragePremium.CoveragePeriod.End;
        public int Order { get; set; }
        public bool FromReinstatement { get; protected set; } = false;

        public abstract object Clone();

        public bool IsDifferentAnnualPremium(RiskPremiumBase<T> riskPremium)
        {
            return CoveragePremium.Premium.Annual != riskPremium.CoveragePremium.Premium.Annual;
        }

        public bool IsDifferentCoveragePeriod(RiskPremiumBase<T> riskPremium)
        {
            return !CoveragePremium.CoveragePeriod.Equals(riskPremium.CoveragePremium.CoveragePeriod);
        }

        public bool IsEarlierThan(RiskPremiumBase<T> riskPremium)
        {
            return EffectiveDate < riskPremium.EffectiveDate;
        }

        public void Expire(DateTime effectiveDate, DateRange policyPeriod)
        {
            CoveragePremium = (T) CoveragePremium.Clone();
            CoveragePremium.Expire(effectiveDate, policyPeriod);
        }

        public void TagAsFromReinstatement()
        {
            FromReinstatement = true;
        }

    }
}