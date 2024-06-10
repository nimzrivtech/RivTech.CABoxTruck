using System;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration
{
    public interface IAmount
    {
        AmountInfo Info { get; }
        decimal Amount { get; }
        decimal CancelledAmount { get; }
        public decimal AmountReduction { get; }
        DateTime? CancellationDate { get; }

        void Cancel(DateTime cancellationDate, bool? isFlatCancel = null);
    }
}
