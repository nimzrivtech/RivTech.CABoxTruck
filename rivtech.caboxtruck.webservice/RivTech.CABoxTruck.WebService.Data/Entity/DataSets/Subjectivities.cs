using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class Subjectivities : Enumeration<short>
    {
         public static Subjectivities Mil300Limit = new Subjectivities(1, "300 Mileage Limitation");
         public static Subjectivities Unlisted = new Subjectivities(2, "Unlisted Driver Exclusion");
         public static Subjectivities Occupant = new Subjectivities(3, "Occupant Hazard Exclusion" );
         public static Subjectivities Age23Permitted = new Subjectivities(4, "No drivers under 23 years old permitted.");
         public static Subjectivities Age23and24shouldMVR = new Subjectivities(5, "All 23 and 24 year old drivers must have a clean MVR.");
         public static Subjectivities Age25up1violation = new Subjectivities(6, "All 25+ year old drivers can only have 1 moving violation or accident in previous 36 months.");
         public static Subjectivities ProofOfDepositSigned = new Subjectivities(7, "Proof of deposit or signed financed agreement required to bind.");
         public static Subjectivities VehCountChangePrice = new Subjectivities(8, "If the vehicle count changes at time of binding, price per unit could change.");
         public static Subjectivities DrvrChangePremium = new Subjectivities(9, "The quote is based on MVRs provided. The premium is subject to change if any of the driver information changes.");
         public static Subjectivities RequiredToBind = new Subjectivities(10, "The following are required to bind: Updated signed application that matches the terms of the accepted quote, signed quote, signed um/uim coverage selection/rejection form, signed affidavit / diligent search form, and signed Trucking Verification Form.");
         public static Subjectivities RequiredRequestedCoverage = new Subjectivities(11, "Copy of the contract with the logistics company required for the requested coverages selected.");
         public static Subjectivities ProofOfSafetyDevice = new Subjectivities(12, "Proof of safety devices, GPS, and/or telematics required.");
         public static Subjectivities UpdatedExcelVehicle = new Subjectivities(13, "Updated Excel Spreadsheet of vehicles required at time of bind.");
         public static Subjectivities UpdatedExcelDriver = new Subjectivities(14, "Updated Excel Spreadsheet of drivers required at time of bind.");
         public static Subjectivities ValueLossRequired = new Subjectivities(15, "Currently valued loss runs required to bind.");
         public static Subjectivities ValuedMvrRequired = new Subjectivities(16, "Currently valued MVRs required to bind.");

        public Subjectivities(short id, string name)
            : base(id, name)
        {
        }

        public Subjectivities() { }
    }
}
