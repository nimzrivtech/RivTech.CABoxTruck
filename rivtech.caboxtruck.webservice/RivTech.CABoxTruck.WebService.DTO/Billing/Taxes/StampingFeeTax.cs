using RivTech.CABoxTruck.WebService.Data.Entity;
using System;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Taxes
{
    public abstract class StampingFeeTax : Tax
    {
        public static StampingFeeTax FromTypeId(string typeId, decimal rate)
        {
            if (typeId == StampingFeeTaxType.Percentage.Id)
            {
                return new StampingFeePercentageTax(rate);
            }
            else if (typeId == StampingFeeTaxType.Flat.Id)
            {
                return new StampingFeeFlatRateTax(rate);
            }
            else return null;
        }

        public StampingFeeTax(decimal rate) : base(rate) { }

        public abstract decimal CalculateTaxAmount(decimal taxableAmounts);
        public bool IsExcluded { get; set; }
    }

    public class StampingFeePercentageTax : StampingFeeTax
    {
        public StampingFeePercentageTax(decimal rate) : base(rate) { }

        public override decimal CalculateTaxAmount(decimal amount)
        {
            return (Rate / 100) * amount;
        }
    }

    public class StampingFeeFlatRateTax : StampingFeeTax
    {
        public StampingFeeFlatRateTax(decimal rate) : base(rate) { }

        public override decimal CalculateTaxAmount(decimal taxableAmounts)
        {
            if (IsExcluded) return 0m;
            return Rate;
        }
    }

    public class StampingFeeTaxType : Enumeration<string>
    {
        public static readonly StampingFeeTaxType Percentage = new StampingFeeTaxType("P", "Percentage");
        public static readonly StampingFeeTaxType Flat = new StampingFeeTaxType("F", "Flat-rate");
        private StampingFeeTaxType(string id, string desc) : base(id, desc) { }
    }

}
