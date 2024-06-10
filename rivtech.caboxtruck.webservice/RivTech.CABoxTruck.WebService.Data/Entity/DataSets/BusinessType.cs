namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class BusinessType : Enumeration<short>
    {
        public static BusinessType Individual = new BusinessType(1, "Individual");
        public static BusinessType SoleProprietorship = new BusinessType(2, "Sole Proprietorship");
        public static BusinessType Partnership = new BusinessType(3, "Partnership");
        public static BusinessType Corporation = new BusinessType(4, "Corporation");
        public static BusinessType Llc = new BusinessType(5, "LLC");
        public static BusinessType SCorp = new BusinessType(6, "S-Corp");
        public static BusinessType Other = new BusinessType(7, "Other");
        public static BusinessType GovernmentEntity = new BusinessType(8, "Government Entity");
        public static BusinessType JointVenture = new BusinessType(9, "Joint Venture");

        public BusinessType(short id, string name)
            : base(id, name)
        {
        }

        public BusinessType()
        {
        }
    }
}