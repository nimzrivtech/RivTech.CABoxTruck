using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration
{
    public class ProratedAmount : IAmount
    {
        #region Static Factory Methods

        /// <summary>
        /// Creates a new ProratedPremium object with a single period. 
        /// Good for premiums that can only have one annual premium such as Additional Premiums or Prorated Manuscripts.
        /// </summary>
        public static ProratedAmount CreateWithSinglePeriods(AmountInfo info, ProratedAmountPeriod premiumPeriod, DateRange policyPeriod)
        {
            if (premiumPeriod is null)
                throw new ArgumentNullException($"{nameof(premiumPeriod)}");
            var proratedAmount = new ProratedAmount(info, policyPeriod);
            proratedAmount._periods.AddLast(premiumPeriod);
            return proratedAmount;
        }

        /// <summary>
        /// Creates a new ProratedPremium object with many periods that have different annual premiums. 
        /// </summary>
        public static ProratedAmount CreateWithManyPeriods(AmountInfo info, IEnumerable<ProratedAmountPeriod> periods, DateRange policyPeriod)
        {
            periods ??= new List<ProratedAmountPeriod>();
            periods = periods.Where(p => p != null);
            var proratedAmount = new ProratedAmount(info, policyPeriod);

            foreach (var p in periods.OrderByDescending(p => p.EffectivityPeriod.Start))
                proratedAmount._periods.AddLast(p);

            return proratedAmount;
        }

        #endregion

        private readonly LinkedList<ProratedAmountPeriod> _periods = new LinkedList<ProratedAmountPeriod>();

        public ProratedAmount(AmountInfo info, DateRange policyPeriod)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            PolicyPeriod = policyPeriod ?? throw new ArgumentNullException(nameof(policyPeriod));
        }

        public AmountInfo Info { get; }
        public IEnumerable<ProratedAmountPeriod> Periods => _periods.ToList().AsReadOnly();
        public DateRange PolicyPeriod { get; }
        public decimal MinimumPercentage { get; set; }
        public bool WithShortRate { get; set; }
        public bool IsFlatCancel { get; private set; }
        public decimal AnnualMinimumEarned => _periods.FirstOrDefault(p => p.FromSubmission)?.AnnualAmount ?? 0;
        public decimal MinimumEarned {
            get
            {
                if (IsFlatCancel)
                    return 0;
                return (AnnualMinimumEarned * MinimumPercentage / 100);
            }
        }
        public decimal MinimumEarnedAdjustment => IsFlatCancel ? 0 : (MinimumEarned - CancelledAmount); //  || (CancelledAmount > MinimumEarned)
        /// <summary>
        /// 10% of difference between total prorated premium and cancelled premium amount.
        /// </summary>
        public decimal ShortRatePremium => WithShortRate ? Math.Abs(AmountReduction * 0.1m) : 0;
        public decimal ShortRatePremiumRounded => Math.Round(ShortRatePremium, 2, MidpointRounding.AwayFromZero);
        public decimal MinimumEarnedAdjustmentRounded => Math.Round(MinimumEarnedAdjustment, 2, MidpointRounding.AwayFromZero);  
        public decimal Amount => _periods.Sum(p => p.ActualAmount);
        public decimal AmountReduction => CancelledAmount - Amount;
        public decimal AmountRounded => Math.Round(Amount, 2, MidpointRounding.AwayFromZero);

        public DateTime? CancellationDate { get; private set; }
        public bool IsCancelled => CancellationDate.HasValue;
        public decimal CancelledAmount
        {
            get
            {
                if (!IsCancelled || IsFlatCancel) return 0;

                decimal totalCancelledAmount = 0;

                var currentPeriodNode = _periods.First;
                while(currentPeriodNode != null)
                {
                    ProratedAmountPeriod period = currentPeriodNode.Value;
                    if (period.EffectivityPeriod.IsDateWithinRange(CancellationDate.Value))
                    {
                        totalCancelledAmount += period.CalculateAmountWithDate(CancellationDate.Value);
                    }
                    else if (period.EffectivityPeriod.IsEarlierThan(CancellationDate.Value))
                    {
                        totalCancelledAmount += period.ActualAmount;
                    }

                    currentPeriodNode = currentPeriodNode.Next;
                } 

                return totalCancelledAmount;
            }
        }

        public decimal CancelledAmountRounded => Math.Round(CancelledAmount, 2, MidpointRounding.AwayFromZero);

        public void Cancel(DateTime cancellationDate, bool? isFlatCancel = null)
        {
            if (!PolicyPeriod.IsDateWithinRange(cancellationDate))
                throw new ArgumentOutOfRangeException("Cancellation date must be within policy period.");
            CancellationDate = cancellationDate;
            IsFlatCancel = isFlatCancel ?? false;
        }

        public override string ToString()
        {
            var text = $" {Info},\tAmount: {AmountRounded:C},\tCancelled Amount: {CancelledAmount:C},\tCancellation Date: {CancellationDate:MM-dd-yyyy},\tMinimum Premium: {MinimumEarned:C},\tMin. Prem. Adj.: {MinimumEarnedAdjustment:C}";
            foreach (ProratedAmountPeriod p in _periods)
                text += $"\n{p}";
            return text;
        }
    }
}