using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class QuoteConditionsOption : Enumeration<short>
    {
        public static QuoteConditionsOption quoteOpt4 = new QuoteConditionsOption(4, "No drivers under 23 years old are permitted.");
        public static QuoteConditionsOption quoteOpt5 = new QuoteConditionsOption(5, "All 23 and 24 year old drivers must have a clean MVR.");
        public static QuoteConditionsOption quoteOpt6 = new QuoteConditionsOption(6, "All 25+ year old drivers can only have 1 moving violation or accident in previous 36 months.");
        public static QuoteConditionsOption quoteOpt7 = new QuoteConditionsOption(7, "If the vehicle count changes at time of binding, price per unit is subject to change.");
        public static QuoteConditionsOption quoteOpt8 = new QuoteConditionsOption(8, "This quote is based on MVRs provided. The premium is subject to change if any of the driver information changes.");
        public static QuoteConditionsOption quoteOpt9 = new QuoteConditionsOption(9, "All drivers must have 2+ years experience with similar commercial unit.");
        public static QuoteConditionsOption quoteOpt10 = new QuoteConditionsOption(10, "Other");
        public static QuoteConditionsOption quoteOpt11 = new QuoteConditionsOption(11, "No authority is granted to the producer to bind insurance coverage.");
        public static QuoteConditionsOption quoteOpt12 = new QuoteConditionsOption(12, "The attached proposal is valid only if presented prior to expiration, in its original form, and in its entirety without modification.");
        public static QuoteConditionsOption quoteOpt13 = new QuoteConditionsOption(13, "Policy includes a Physical Damage Insurance Limit of Insurance.");
        public QuoteConditionsOption(short id, string name)
            : base(id, name)
        {
        }

        public QuoteConditionsOption() { }
    }
}
