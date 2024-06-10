using System;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration
{
    public class FullyEarnedAmount : IAmount
    {
        public FullyEarnedAmount(AmountInfo info, decimal premium)
        {
            Info = info;
            Amount = premium;
        }

        public AmountInfo Info { get; }
        public decimal Amount { get; }
        public decimal CancelledAmount => IsFlatCancel ? 0 : Amount;
        public decimal AmountReduction => CancelledAmount - Amount;
        public bool IsFlatCancel { get; private set; }
        public DateTime? CancellationDate { get; private set; }

        public void Cancel(DateTime cancellationDate, bool? isFlatCancel = null)
        {
            CancellationDate = cancellationDate;
            IsFlatCancel = isFlatCancel ?? false;
        }

        public override string ToString()
        {
            return $"{Info}, Amount: {Amount:C}";
        }
    }
}
