using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class BackgroundCheckOption : Enumeration<short>
    {
        public static BackgroundCheckOption None = new BackgroundCheckOption(1, "None");
        public static BackgroundCheckOption WrittenApplication = new BackgroundCheckOption(2, "Written Application");
        public static BackgroundCheckOption RoadTest = new BackgroundCheckOption(3, "Road Test");
        public static BackgroundCheckOption WrittenTest = new BackgroundCheckOption(4, "Written Test");
        public static BackgroundCheckOption FullMedical = new BackgroundCheckOption(5, "Full Medical");
        public static BackgroundCheckOption DrugTesting = new BackgroundCheckOption(6, "Drug Testing");
        public static BackgroundCheckOption CurrentMvr = new BackgroundCheckOption(7, "Current MVR");
        public static BackgroundCheckOption ReferenceChecks = new BackgroundCheckOption(8, "Reference Checks");
        public static BackgroundCheckOption CriminalBackgroundCheck = new BackgroundCheckOption(9, "Criminal Background Check");

        public List<RiskSpecificDriverHiringCriteria> RiskSpecificDriverHiringCriteria { get; set; }

        public BackgroundCheckOption(short id, string name)
            : base(id, name)
        {
        }

        public BackgroundCheckOption() { }
    }
}
