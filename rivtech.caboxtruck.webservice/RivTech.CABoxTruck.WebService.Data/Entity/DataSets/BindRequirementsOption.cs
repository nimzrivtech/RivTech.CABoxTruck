using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class BindRequirementsOption : Enumeration<short>
    {
        public static BindRequirementsOption bindreq1 = new BindRequirementsOption(1, "Proof of deposit or signed financed agreement");
        public static BindRequirementsOption bindreq2 = new BindRequirementsOption(2, "Updated signed application matching the terms of accepted quote");
        public static BindRequirementsOption bindreq3 = new BindRequirementsOption(3, "Signed Quote");
        public static BindRequirementsOption bindreq4 = new BindRequirementsOption(4, "Signed UM/UIM coverage selection / rejection form");
        public static BindRequirementsOption bindreq5 = new BindRequirementsOption(5, "Signed affidavit / diligent search form");
        public static BindRequirementsOption bindreq6 = new BindRequirementsOption(6, "Signed trucking verification form");
        public static BindRequirementsOption bindreq7 = new BindRequirementsOption(7, "Copy of the contract with the logistics company required for the requested coverages selected.");
        public static BindRequirementsOption bindreq8 = new BindRequirementsOption(8, "Proof of safety devices, GPS, and/or telematics");
        public static BindRequirementsOption bindreq9 = new BindRequirementsOption(9, "Updated Excel Spreadsheet of vehicles at bind");
        public static BindRequirementsOption bindreq10 = new BindRequirementsOption(10, "Updated Excel Spreadsheet of drivers at bind");
        public static BindRequirementsOption bindreq11 = new BindRequirementsOption(11, "Currently valued loss runs required to bind.");
        public static BindRequirementsOption bindreq12 = new BindRequirementsOption(12, "Currently valued MVRs required to bind.");
        public static BindRequirementsOption bindreq13 = new BindRequirementsOption(13, "Signed Box Truck Supplemental Application for 1-3 Unit Accounts");
        public static BindRequirementsOption bindreq14 = new BindRequirementsOption(14, "Signed Acord Applications for 1-3 Unit Accounts");
        public static BindRequirementsOption bindreq15 = new BindRequirementsOption(15, "Signed PIP Selection form");
        public static BindRequirementsOption bindreq16 = new BindRequirementsOption(16, "Other");

        public BindRequirementsOption(short id, string name)
            : base(id, name)
        {
        }

        public BindRequirementsOption() { }
    }
}
