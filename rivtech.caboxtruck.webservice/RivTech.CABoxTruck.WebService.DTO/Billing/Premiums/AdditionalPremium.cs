using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Premiums
{
    public class AdditionalPremium : IPremium
    {
        public AdditionalPremium(AdditionalPremiumKind kind, decimal value)
        {
            Kind = kind;
            Value = value;
        }

        public AdditionalPremiumKind Kind { get; }
        public decimal Value { get; }
        public string Description => Kind.Description;

    }

    public class AdditionalPremiumKind : Enumeration<string>
    {
        public static readonly AdditionalPremiumKind AdditionalInsured = new AdditionalPremiumKind("AI", "Additional Insured Premium");
        public static readonly AdditionalPremiumKind WaiverOfSubrogation = new AdditionalPremiumKind("WOS", "Waiver of Subrogation Premium");
        public static readonly AdditionalPremiumKind PrimaryAndNonContributory = new AdditionalPremiumKind("PNC", "Primary & Non-Contributory Premium");
        public static readonly AdditionalPremiumKind GeneralLiability = new AdditionalPremiumKind("GL", "General Liability");

        private AdditionalPremiumKind(string id, string description) : base(id, description)
        {
        }

    }

}
