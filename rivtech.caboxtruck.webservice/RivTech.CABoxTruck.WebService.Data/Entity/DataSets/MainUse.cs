namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class MainUse : Enumeration<short>
    {
        public static MainUse Courier = new MainUse(1, "Courier");
        public static MainUse Commercial = new MainUse(2, "Commercial");
        public static MainUse Service = new MainUse(3, "Service");
        public static MainUse HeavyLoadsSpecial = new MainUse(4, "Heavy Loads/Special");
        public static MainUse Logging = new MainUse(5, "Logging");
        public static MainUse NonBusiness = new MainUse(6, "Non-Business");
        public static MainUse PizzaPreparedFood = new MainUse(7, "Pizza/Prepared Food");
        public static MainUse Funeral = new MainUse(8, "Funeral");
        public static MainUse ChildCare = new MainUse(9, "Child Care");
        public static MainUse Church = new MainUse(10, "Church");
        public static MainUse SocialServicePassengerTransport = new MainUse(11, "Social Service Passenger Transport");
        public static MainUse Livery = new MainUse(12, "Livery");
        public static MainUse Repossession = new MainUse(13, "Repossession");
        public static MainUse CoalHauling = new MainUse(14, "Coal Hauling");
        public static MainUse Towing247 = new MainUse(15, "Towing 24/7");


        public MainUse(short id, string name)
            : base(id, name)
        {
        }

        public MainUse()
        {
        }
    }
}