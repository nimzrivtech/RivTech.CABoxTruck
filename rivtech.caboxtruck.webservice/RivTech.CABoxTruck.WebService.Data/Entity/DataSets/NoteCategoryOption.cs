using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class NoteCategoryOption : Enumeration<short>
    {
        public static NoteCategoryOption Underwriting = new NoteCategoryOption(1, "Underwriting");
        public static NoteCategoryOption Finance = new NoteCategoryOption(2, "Finance");
        public static NoteCategoryOption Policy = new NoteCategoryOption(3, "Policy");
        public static NoteCategoryOption Claims = new NoteCategoryOption(4, "Claims");
        public static NoteCategoryOption Miscellaneous = new NoteCategoryOption(5, "Miscellaneous");
        public static NoteCategoryOption Billing = new NoteCategoryOption(6, "Billing");
        public static NoteCategoryOption TaskNotes = new NoteCategoryOption(7, "Task Notes");

        public NoteCategoryOption(short id, string name)
            : base(id, name)
        {
        }

        public NoteCategoryOption() { }
    }
}
