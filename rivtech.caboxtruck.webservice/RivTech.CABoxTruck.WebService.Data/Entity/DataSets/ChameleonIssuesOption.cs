using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class ChameleonIssuesOption : Enumeration<short>
    {
        public static ChameleonIssuesOption None = new ChameleonIssuesOption(1, "None");
        public static ChameleonIssuesOption OneMajor = new ChameleonIssuesOption(2, "1 - Major");
        public static ChameleonIssuesOption OneMinor = new ChameleonIssuesOption(3, "1 - Minor");
        public static ChameleonIssuesOption MultipleMajor = new ChameleonIssuesOption(4, "Multiple - Major");
        public static ChameleonIssuesOption MultipleMinor = new ChameleonIssuesOption(5, "Multiple - Minor");

        public List<RiskSpecificDotInfo> RiskSpecificDotInfo { get; set; }

        public ChameleonIssuesOption(short id, string name)
            : base(id, name)
        {
        }

        public ChameleonIssuesOption() { }
    }
}
