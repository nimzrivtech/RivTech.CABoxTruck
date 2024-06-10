namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class Symbol : Enumeration<short>
    {
        public static Symbol Symbol789 = new Symbol(1, "7, 8 & 9");
        public static Symbol Symbol79 = new Symbol(2, "7 & 9");
        public static Symbol Symbol289 = new Symbol(3, "2, 8 & 9");
        public static Symbol Symbol1 = new Symbol(4, "1) Any Auto");
        public static Symbol Symbol2 = new Symbol(5, "2) All Owned Autos");
        public static Symbol Symbol3 = new Symbol(6, "3) Owned Private Passenger Autos");
        public static Symbol Symbol4 = new Symbol(7, "4) Owned Autos Other Than Private Passenger");
        public static Symbol Symbol5 = new Symbol(8, "5) All Owned Autos Which Require No-Fault Coverage");
        public static Symbol Symbol6 = new Symbol(9, "6) Owned Autos Subject to Compulsory UM Law");
        public static Symbol Symbol7 = new Symbol(10, "7) Autos Specified on  Schedule");
        public static Symbol Symbol8 = new Symbol(11, "8) Hired Autos");
        public static Symbol Symbol9 = new Symbol(12, "9) Non-Owned Autos");
        public static Symbol Symbol78 = new Symbol(13, "7 & 8");
        public static Symbol Symbol10_1 = new Symbol(14, "10 - Use 2, 8 & 9 Pricing");
        public static Symbol Symbol10_2 = new Symbol(15, "10 - Use 7, 8 & 9 Pricing");

        public Symbol(short id, string name)
            : base(id, name)
        {
        }

        public Symbol()
        {
        }
    }
}