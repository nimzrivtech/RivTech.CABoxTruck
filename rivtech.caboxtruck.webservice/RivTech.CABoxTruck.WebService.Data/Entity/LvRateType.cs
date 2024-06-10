using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvRateType : BaseEntity<byte>, IDescription, IIsActive
    {
        public LvRateType()
        {
            this.IsActive = true;
        }

        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
