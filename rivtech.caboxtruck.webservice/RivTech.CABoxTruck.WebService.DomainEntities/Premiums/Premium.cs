using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public class Premium
    {
        public static readonly string TYPE_POLICY_GL = "GL";
        public static readonly string TYPE_POLICY_TI = "TI";
        public static readonly string TYPE_POLICY_NO = "NO";

        private Premium() { }

        public Premium(decimal annual)
        {
            Annual = annual;
        }

        public Premium(decimal annual, decimal prorated)
        {
            Annual = annual;
            Prorated = prorated;
        }

        public decimal Annual { get; private set; }
        public decimal Prorated { get; private set; }

        /// <summary>
        /// Recalculates prorated property with given coverage & policy period.
        /// </summary>
        public Premium Prorate(DateRange coveragePeriod, DateRange policyPeriod)
        {
            var prorated = Annual * coveragePeriod.DaysNonNegative / policyPeriod.DaysNonNegative;
            return new Premium
            {
                Annual = Annual,
                Prorated = prorated
            };
        }

        internal Premium ChangeAnnual(decimal annual)
        {
            return new Premium
            {
                Annual = annual,
                Prorated = 0m
            };
        }

        public static Premium Empty()
        {
            return new Premium()
            {
                Annual = 0,
                Prorated = 0
            };
        }

        public Premium Clone()
        {
            return new Premium
            {
                Annual = Annual,
                Prorated = Prorated
            };
        }

        public override string ToString()
        {
            return $"Annual: {Annual}, Prorated: {Prorated}";
        }
    }
}
