using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class CreditedOffice : Enumeration<short>
    {
        public static CreditedOffice Rivington = new CreditedOffice(1, "Rivington");
        public static CreditedOffice Vale = new CreditedOffice(2, "Vale");
        public static CreditedOffice Applied = new CreditedOffice(3, "Applied");

        public CreditedOffice(short id, string name)
            : base(id, name)
        {
        }

        public CreditedOffice() { }
    }
}
