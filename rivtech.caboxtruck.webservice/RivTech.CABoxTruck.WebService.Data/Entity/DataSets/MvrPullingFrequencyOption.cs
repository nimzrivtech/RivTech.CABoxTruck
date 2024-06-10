namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class MvrPullingFrequencyOption : Enumeration<short>
    {
        public static MvrPullingFrequencyOption Quarterly = new MvrPullingFrequencyOption(1, "Quarterly");
        public static MvrPullingFrequencyOption SemiAnnually = new MvrPullingFrequencyOption(2, "Semi-Annually");
        public static MvrPullingFrequencyOption Annually = new MvrPullingFrequencyOption(3, "Annually");
        public static MvrPullingFrequencyOption BiAnnually = new MvrPullingFrequencyOption(4, "Bi-Annually");
        public static MvrPullingFrequencyOption NotPulled = new MvrPullingFrequencyOption(5, "Not Pulled");

        public MvrPullingFrequencyOption(short id, string name)
            : base(id, name)
        {
        }

        public MvrPullingFrequencyOption() { }
    }
}
