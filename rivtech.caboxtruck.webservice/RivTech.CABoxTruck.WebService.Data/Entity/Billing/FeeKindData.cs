using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Billing
{
    public class FeeKindData
    {
        public string Id { get; }
        public string Description { get; }
        public bool IsActive { get; } = true;
    }
}
