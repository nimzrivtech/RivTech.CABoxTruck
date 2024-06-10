using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class BindStatus : Enumeration<short>
    {
        public static BindStatus Pending = new BindStatus(1, "Pending");
        public static BindStatus Complete = new BindStatus(2, "Complete");

        public BindStatus(short id, string name)
            : base(id, name)
        {
        }

        public BindStatus() { }
    }
}
