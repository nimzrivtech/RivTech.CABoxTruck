namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class RiskStatus : Enumeration<string>
    {
        public static readonly RiskStatus Received = new RiskStatus("R", "Received", true);
        public static readonly RiskStatus InResearch = new RiskStatus("In", "InResearch", true);
        public static readonly RiskStatus InComplete = new RiskStatus("INC", "InComplete", true);
        public static readonly RiskStatus Complete = new RiskStatus("C", "Complete", true);
        public static readonly RiskStatus Withdrawn = new RiskStatus("W", "Withdrawn", true);

        public RiskStatus()
        {
            
        }

        public RiskStatus(string id, string description, bool isActive) : base(id, description)
        {
            
        }
    }
}
