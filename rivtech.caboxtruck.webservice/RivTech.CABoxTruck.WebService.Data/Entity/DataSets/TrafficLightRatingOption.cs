using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class TrafficLightRatingOption : Enumeration<short>
    {
        public static TrafficLightRatingOption Red = new TrafficLightRatingOption(1, "Red");
        public static TrafficLightRatingOption Yellow = new TrafficLightRatingOption(2, "Yellow");
        public static TrafficLightRatingOption Green = new TrafficLightRatingOption(3, "Green");
        public static TrafficLightRatingOption NoInfo = new TrafficLightRatingOption(4, "No Info");

        public List<RiskSpecificDotInfo> RiskSpecificDotInfo { get; set; }

        public TrafficLightRatingOption(short id, string name)
            : base(id, name)
        {
        }

        public TrafficLightRatingOption() { }
    }
}
