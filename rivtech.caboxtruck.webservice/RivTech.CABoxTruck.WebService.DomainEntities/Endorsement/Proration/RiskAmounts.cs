using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration
{
    public class RiskAmounts
    {
        private readonly List<IAmount> _amounts = new List<IAmount>();

        public IEnumerable<IAmount> Amounts => _amounts.AsReadOnly();
        public DateTime? CancellationDate { get; private set; }

        public RiskAmounts(DateRange policyPeriod, decimal minimumEarnedPercentage)
        {
            PolicyPeriod = policyPeriod;
            MinimumEarnedPercentage = minimumEarnedPercentage;
        }

        public bool WithShortRate { get; private set; } = false;

        public DateRange PolicyPeriod { get; }
        public decimal MinimumEarnedPercentage { get; }
        public bool IsFlatCancel { get; private set; }
        public decimal MinimumEarnedPercentageInDecimal => MinimumEarnedPercentage / 100;

        public decimal TotalCancelledAmount
        {
            get
            {
                if (!CancellationDate.HasValue)
                    return 0;
                return Amounts.Sum(p => p.CancelledAmount);
            }
        }
        public decimal TotalPremium => _amounts.Sum(p => p.Amount);
        protected IEnumerable<ProratedAmount> ProratedPremiums => Amounts.Where(p => p is ProratedAmount)?.Select(p => p as ProratedAmount);
        protected IEnumerable<FullyEarnedAmount> FullyEarnedPremiums => Amounts.Where(p => p is FullyEarnedAmount)?.Select(p => p as FullyEarnedAmount);
        public decimal TotalMinimumPremium => ProratedPremiums.Sum(p => p.MinimumEarned) + FullyEarnedPremiums.Sum(p => p.MinimumEarned);
        public decimal TotalMinimumPremiumAdjustmentCalc => TotalMinimumPremium - TotalCancelledAmount;
        public decimal TotalMinimumPremiumAdjustment => ProratedPremiums?.Sum(p => p.MinimumEarnedAdjustment) ?? 0;
        public decimal TotalShortRatePremium => ProratedPremiums?.Sum(p => p.ShortRatePremium) ?? 0;
        public decimal FinalPremium
        {
            get
            {
                var premium = TotalCancelledAmount + TotalMinimumPremiumAdjustment;
                if(WithShortRate)
                {
                    premium += TotalShortRatePremium;
                }
                return premium;
            }
        }

        public decimal TotalCancelledAmountRounded => Math.Round(TotalCancelledAmount, 2, MidpointRounding.AwayFromZero);
        public decimal TotalPremiumRounded => Math.Round(TotalPremium, 2, MidpointRounding.AwayFromZero);
        public decimal TotalMinimumPremiumRounded => Math.Round(TotalMinimumPremiumAdjustment, 2, MidpointRounding.AwayFromZero);
        public decimal TotalShortRatePremiumAdjustmentRounded => Math.Round(TotalShortRatePremium, 2, MidpointRounding.AwayFromZero);

        public void Add(IAmount premium)
        {
            if (premium is null)
                return;
            _amounts.Add(premium);
            System.Diagnostics.Debug.WriteLine(premium);
        }

        public void Add(IEnumerable<IAmount> premiums)
        {
            foreach (var premium in premiums)
                Add(premium);
        }

        public void RemovePremium(IAmount premium)
        {
            if (premium is null)
                return;
            _amounts.Remove(premium);
        }

        public void Cancel(DateTime cancellationDate, bool? withShortRate = null, bool? isFlatCancel = null)
        {
            if (!PolicyPeriod.IsDateWithinRange(cancellationDate))
                throw new InvalidOperationException("Cancellation date must be within policy period.");

            CancellationDate = cancellationDate;
            WithShortRate = withShortRate ?? false;
            foreach (var amount in _amounts)
            {
                if (amount is ProratedAmount proratedAmount)
                {
                    proratedAmount.MinimumPercentage = MinimumEarnedPercentage;
                    proratedAmount.WithShortRate = withShortRate ?? false;
                }

                if (amount is FullyEarnedAmount fullyEarnedAmount)
                {
                    fullyEarnedAmount.MinimumPercentage = MinimumEarnedPercentage;
                }
                amount.Cancel(cancellationDate, isFlatCancel);
                Console.WriteLine(amount);
            }
        }
    }
}
