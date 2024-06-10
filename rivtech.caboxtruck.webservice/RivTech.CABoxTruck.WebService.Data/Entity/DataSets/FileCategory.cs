using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class FileCategory : Enumeration<short>
    {
        public static FileCategory Underwriting = new FileCategory(1, "Underwriting");
        public static FileCategory Quoting = new FileCategory(2, "Quoting");
        public static FileCategory Billing = new FileCategory(3, "Billing");
        public static FileCategory BindRequirement = new FileCategory(4, "Bind Requirement");
        public static FileCategory QuoteCondition = new FileCategory(5, "Quote Condition");
        public static FileCategory MidtermChange = new FileCategory(6, "Midterm Change");
        public static FileCategory BrokerCorrespondence = new FileCategory(7, "Broker Correspondence");
        public static FileCategory Filings = new FileCategory(8, "Filings");

        public FileCategory(short id, string name)
            : base(id, name)
        {
        }

        public FileCategory() { }
    }
}
