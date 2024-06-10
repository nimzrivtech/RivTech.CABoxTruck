using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Premiums
{
    public class RegularPremium : IPremium
    {
        public RegularPremium(RegularPremiumKind kind, decimal value)
        {
            Kind = kind;
            Value = value;
        }

        public RegularPremiumKind Kind { get; }

        public decimal Value { get; }

        public string Description => Kind.Description;
    }

    public class RegularPremiumKind : Enumeration<string>
    {
        public static readonly RegularPremiumKind AutoLiability = new RegularPremiumKind("ALPREM", "Auto Liability Premium");
        public static readonly RegularPremiumKind PhysicalDamage = new RegularPremiumKind("PDPREM", "Physical Damage Premium");
        public static readonly RegularPremiumKind Cargo = new RegularPremiumKind("CRGPREM", "Cargo Premium");

        private RegularPremiumKind(string id, string description) : base(id, description)
        {
        }
    }

}
