using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Taxes
{
    public abstract class Tax
    {
        public Tax(decimal rate)
        {
            Rate = rate;
        }

        public decimal Rate { get; }
    }

    public class TaxType : Enumeration<string>
    {
        public static readonly TaxType SurplusLine = new TaxType("SL", "Surplus Line");
        public static readonly TaxType StampingFee = new TaxType("SF", "Stamping Fee");

        private TaxType(string id, string desc) : base(id, desc) { }
    }

}
