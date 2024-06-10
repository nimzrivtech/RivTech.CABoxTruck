using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Billing
{
    public class FeeDetails : BaseEntity<int>, IIsActive
    {
        public FeeDetails()
        {
            IsActive = true;
        }

        public string StateCode { get; set; }
        public string FeeKindId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
    }
}
