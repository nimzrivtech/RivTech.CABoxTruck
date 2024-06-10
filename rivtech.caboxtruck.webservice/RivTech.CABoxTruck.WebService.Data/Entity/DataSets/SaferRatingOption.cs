using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class SaferRatingOption : Enumeration<short>
    {
        public static SaferRatingOption InViolation = new SaferRatingOption(1, "In Violation");
        public static SaferRatingOption Conditional = new SaferRatingOption(2, "Conditional");
        public static SaferRatingOption Satisfactory = new SaferRatingOption(3, "Satisfactory");
        public static SaferRatingOption NotNeeded = new SaferRatingOption(4, "Not Rated");

        public List<RiskSpecificDotInfo> RiskSpecificDotInfo { get; set; }

        public SaferRatingOption(short id, string name)
            : base(id, name)
        {
        }

        public SaferRatingOption() { }
    }
}
