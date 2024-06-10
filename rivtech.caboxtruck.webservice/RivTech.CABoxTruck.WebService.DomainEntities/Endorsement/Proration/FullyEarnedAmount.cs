using System;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration
{
    public class FullyEarnedAmount : IAmount
    {
        public FullyEarnedAmount(AmountInfo info, decimal premium, bool? fromSubmission = null, bool? isDeleted = null)
        {
            Info = info;
            Amount = premium;
            FromSubmission = fromSubmission ?? false;
            IsDeleted = isDeleted ?? false;
        }

        public AmountInfo Info { get; }
        public decimal MinimumPercentage { get; set; }
        public decimal Amount { get; }
        //public decimal CancelledAmount => IsFlatCancel ? 0 : Amount;
        public bool IsDeleted { get; set; }
        public decimal CancelledAmount
        {
            get
            {
                if (IsFlatCancel) return 0;

                if (IsDeleted && CancellationDate.HasValue)
                {
                    return 0;
                }

                return Amount;
            }
        }
        //public decimal AmountReduction => CancelledAmount - Amount;
        public decimal AmountReduction
        {
            get
            {
                if (IsFlatCancel)
                    return CancelledAmount - Amount;
                return 0;
            }
        }
        public decimal AnnualMinimumEarned
        {
            get
            {
                if (IsFlatCancel || !FromSubmission)
                    return 0;
                return Amount;
            }
        }
        public bool FromSubmission { get; set; } 
        public decimal MinimumEarnedAdjustment { get; set; }
        public decimal MinimumEarned
        {
            get
            {
                if (IsFlatCancel || !FromSubmission)
                    return 0;
                return (AnnualMinimumEarned * MinimumPercentage / 100);
            }
        }
        public bool IsFlatCancel { get; private set; }
        public DateTime? CancellationDate { get; private set; }

        public void Cancel(DateTime cancellationDate, bool? isFlatCancel = null)
        {
            CancellationDate = cancellationDate;
            IsFlatCancel = isFlatCancel ?? false;
        }

        public void CalculateEntityMPA(decimal totalEarnedPremium, decimal totalMPA)
        {
            MinimumEarnedAdjustment = IsFlatCancel || totalEarnedPremium == 0 ? 0 : (CancelledAmount / totalEarnedPremium) * totalMPA;
        }

        public override string ToString()
        {
            return $"{Info}, Amount: {Amount:C}";
        }
    }
}
