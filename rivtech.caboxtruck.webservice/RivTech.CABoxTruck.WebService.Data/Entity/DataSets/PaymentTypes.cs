using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class PaymentTypes : Enumeration<short>
    {
        public static PaymentTypes DirectBillFullPay = new PaymentTypes(1, "Direct Bill Full Pay");
        public static PaymentTypes DirectBillInstallments = new PaymentTypes(2, "Direct Bill Installments");
        public static PaymentTypes PremiumFinanced = new PaymentTypes(3, "Premium Financed");

        public PaymentTypes(short id, string name)
            : base(id, name)
        {
        }

        public PaymentTypes() { }
    }
}
