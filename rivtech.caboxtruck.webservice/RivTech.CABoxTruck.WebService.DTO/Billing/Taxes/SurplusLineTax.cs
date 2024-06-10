using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Taxes
{
    public class SurplusLineTax : Tax
    {
        public SurplusLineTax(SLTaxKind kind, decimal rate) : base(rate)
        {
            Kind = kind;
        }

        public SLTaxKind Kind { get; }

        public decimal MultiplyRate(decimal amount)
        {
            return (Rate / 100) * amount;
        }
    }

    public class SLTaxKind : Enumeration<string>
    {
        public static readonly SLTaxKind Main = new SLTaxKind("Main", "Surplus Line");
        public static readonly SLTaxKind FireMarshal = new SLTaxKind("SLFM", "Fire Marshal");
        //public static readonly SurplusLineTaxType WindpoolFee = new SurplusLineTaxType("SLWP", "Winpool Fee");

        public static SLTaxKind FromId(string taxSubtype)
        {
            if (taxSubtype == Main.Id)
                return Main;
            else if (taxSubtype == FireMarshal.Id)
                return FireMarshal;
            else return null;
        }

        private SLTaxKind() { }

        private SLTaxKind(string id, string description) : base(id, description)
        {
        }

    }

}
