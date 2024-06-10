using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Vehicles
{
    /// <summary>
    /// Value object. 
    /// </summary>
    public class VehiclePremiumItem
    {
        public static VehiclePremiumItem Empty() => new VehiclePremiumItem { Annual = 0 };

        private VehiclePremiumItem() { }

        public VehiclePremiumItem(decimal annual, VehiclePremiumItem previous = null)
        {
            Annual = annual;
            Previous = previous;
        }

        public decimal Annual { get; private set; }
        public decimal GrossProrated { get; private set; }
        public decimal NetProrated { get; private set; }

        public VehiclePremiumItem Previous { get; private set; }

        public VehiclePremiumItem ChangeAnnual(decimal annual)
        {
            var clone = Clone();
            clone.Annual = annual;
            return clone;
        }

        /// <summary>
        /// Returns a new instance with gross prorated premium.
        /// </summary>
        public VehiclePremiumItem Prorate(DateTime effectiveDate, DateTime expiration, DateTime policyEffectiveDate, DateTime policyExpirationDate)
        {
            var premium = Clone();

            if (Previous == null && effectiveDate == policyEffectiveDate)
            {
                premium.GrossProrated = premium.Annual;
                premium.NetProrated = premium.Annual;
                return premium;
            }

            VehiclePremiumItem proratedPrevious = null;
            if (Previous != null)
            {
                proratedPrevious = Previous.ProrateRemove(effectiveDate, policyEffectiveDate, policyExpirationDate);
            }

            decimal dailyPremium = premium.Annual / (decimal)(policyExpirationDate.Date - policyEffectiveDate.Date).TotalDays;
            var premiumEffectiveDays = (expiration.Date - effectiveDate.Date).TotalDays;
            var prorated = (decimal)premiumEffectiveDays * dailyPremium;

            premium.GrossProrated = (proratedPrevious?.GrossProrated ?? 0) + prorated;
            //premium.NetProrated = premium.GrossProrated - (premium.Previous?.GrossProrated ?? 0);

            return premium;
        }

        public VehiclePremiumItem RecalculateNet()
        {
            var clone = Clone();
            clone.NetProrated = clone.GrossProrated - (clone.Previous?.GrossProrated ?? 0);
            return clone;
        }

        /// <summary>
        /// Returns a new instance with decreased gross prorated premium based on given expiration.
        /// </summary>
        public VehiclePremiumItem ProrateRemove(DateTime expiration, DateTime policyEffectiveDate, DateTime policyExpirationDate)
        {
            var premium = Clone();

            decimal dailyPremium = premium.Annual / (decimal)(policyExpirationDate.Date - policyEffectiveDate.Date).TotalDays;
            var premiumDaysToRemove = (policyExpirationDate.Date - expiration.Date).TotalDays;

            decimal removedProrated = (decimal)premiumDaysToRemove * dailyPremium;
            premium.GrossProrated -= removedProrated;

            return premium;
        }

        internal VehiclePremiumItem ChangePrevious(VehiclePremiumItem previous)
        {
            var clone = Clone();
            clone.Previous = previous;
            return clone;
        }

        public VehiclePremiumItem Clone()
        {
            return new VehiclePremiumItem
            {
                Annual = Annual,
                GrossProrated = GrossProrated,
                NetProrated = NetProrated,
                Previous = Previous
            };
        }


        public override string ToString()
        {
            return $"Annual: {Annual}, Gross: {GrossProrated}, Net: {NetProrated}";
        }

        public VehiclePremiumItem SetCancelAmount(decimal amount)
        {
            var premium = Clone();
            premium.NetProrated = amount - premium.GrossProrated;
            premium.GrossProrated = amount;
            return premium;
        }

        internal VehiclePremiumItem ChangeGross(decimal grossProrated)
        {
            var premium = Clone();
            premium.GrossProrated = grossProrated;
            return premium;
        }

        internal VehiclePremiumItem ChangePreviousGross(decimal grossProrated)
        {
            var premium = Clone();
            premium.Previous = premium.Previous?.ChangeGross(grossProrated);
            return premium;
        }
    }
}
