using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class PolicyContactTypes : Enumeration<short>
    {
        public static PolicyContactTypes Billing = new PolicyContactTypes(1, "Billing");
        public static PolicyContactTypes Broker = new PolicyContactTypes(2, "Broker");
        public static PolicyContactTypes Claim = new PolicyContactTypes(3, "Claim");
        public static PolicyContactTypes Owner = new PolicyContactTypes(4, "Owner");
        public static PolicyContactTypes Policy = new PolicyContactTypes(5, "Policy");

        public PolicyContactTypes(short id, string description) : base (id, description) { }
        
        public PolicyContactTypes() { }
    }
}
