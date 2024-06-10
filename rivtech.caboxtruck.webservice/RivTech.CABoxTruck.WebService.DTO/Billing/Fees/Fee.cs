using RivTech.CABoxTruck.WebService.Data.Entity;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Fees
{
    public class Fee : ValueObject, IAmount
    {
        public Fee(FeeKind kind, decimal value)
        {
            Kind = kind;
            Value = value;
        }

        public FeeKind Kind { get; }
        public decimal Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Kind;
            yield return Value;
        }
    }


}
