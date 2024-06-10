namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class AdditionalInterestType : Enumeration<short>
    {
        public static AdditionalInterestType AdditionalInsured = new AdditionalInterestType(1, "Additional Insured");
        public static AdditionalInterestType LossPayee = new AdditionalInterestType(2, "Loss Payee");
        public static AdditionalInterestType BreachOfWarranty = new AdditionalInterestType(3, "Breach of Warranty");
        public static AdditionalInterestType Mortgagee = new AdditionalInterestType(4, "Mortgagee");
        public static AdditionalInterestType CoOwner = new AdditionalInterestType(5, "Co-Owner");
        public static AdditionalInterestType Owner = new AdditionalInterestType(6, "Owner");
        public static AdditionalInterestType EmployeeAsLessor = new AdditionalInterestType(7, "Employee As Lessor");
        public static AdditionalInterestType Registrant = new AdditionalInterestType(8, "Registrant");
        public static AdditionalInterestType LeasebackOwner = new AdditionalInterestType(9, "Leaseback Owner");
        public static AdditionalInterestType Trustee = new AdditionalInterestType(10, "Trustee");
        public static AdditionalInterestType Lienholder = new AdditionalInterestType(11, "Lienholder");
        public static AdditionalInterestType DesignatedInsured = new AdditionalInterestType(12, "Designated Insured");

        public AdditionalInterestType() { }

        public AdditionalInterestType(short id, string description) : base(id, description)
        {
        }
    }
}
