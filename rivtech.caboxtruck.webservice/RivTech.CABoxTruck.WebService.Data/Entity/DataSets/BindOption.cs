using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class BindOption : Enumeration<short>
    {
        public static BindOption Option1 = new BindOption(1, "Option 1");
        public static BindOption Option2 = new BindOption(2, "Option 2");
        public static BindOption Option3 = new BindOption(3, "Option 3");
        public static BindOption Option4 = new BindOption(4, "Option 4");

        public BindOption(short id, string name)
            : base(id, name)
        {
        }

        public BindOption() { }
    }
}
