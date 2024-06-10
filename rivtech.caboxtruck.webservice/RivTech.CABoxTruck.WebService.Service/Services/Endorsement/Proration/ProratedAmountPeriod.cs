using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration
{
    /// <summary>
    /// 
    /// </summary>
    public class ProratedAmountPeriod
    {
        public ProratedAmountPeriod(object id, decimal annualAmount, DateRange effectivityPeriod, DateRange policyPeriod, bool? fromSubmission = null)
        {
            Id = id;
            AnnualAmount = annualAmount;
            EffectivityPeriod = effectivityPeriod ?? throw new ArgumentNullException(nameof(effectivityPeriod));
            PolicyPeriod = policyPeriod ?? throw new ArgumentNullException(nameof(policyPeriod));
            FromSubmission = fromSubmission ?? false;
        }

        /// <summary>
        /// Current annual amount.
        /// </summary>
        public decimal AnnualAmount { get; }

        /// <summary>
        /// The total daily amount. 
        /// </summary>
        public decimal ActualAmount => EffectivityPeriod.Days * DailyAmount;
        public decimal ActualAmountRounded => Math.Round(ActualAmount, 2, MidpointRounding.AwayFromZero);
        public bool FromSubmission { get; private set; }

        /// <summary>
        /// Effective date range of premium.
        /// </summary>
        public DateRange EffectivityPeriod { get; }

        /// <summary>
        /// Premium per day based from Annual Premium.
        /// </summary>
        public decimal DailyAmount
        {
            get
            {
                if (PolicyPeriod.Days == 0) return 0;
                return AnnualAmount / PolicyPeriod.Days;
            }
        }

        public object Id { get; }

        /// <summary>
        /// Effectivity date range of Policy.
        /// </summary>
        public DateRange PolicyPeriod { get; }

        public decimal CalculateAmountWithDate(DateTime date)
        {
            var dateRange = EffectivityPeriod.WithNewEnd(date);
            return DailyAmount * dateRange.Days;
        }

        public override string ToString()
        {
            return $"Annual: {AnnualAmount:C},\tActual:{ActualAmount:C},\tEffectivity: {EffectivityPeriod}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as ProratedAmountPeriod;
            return other != null
                && other.AnnualAmount == AnnualAmount
                && other.EffectivityPeriod.Equals(EffectivityPeriod)
                && other.PolicyPeriod .Equals(PolicyPeriod);
        }
    }
}
