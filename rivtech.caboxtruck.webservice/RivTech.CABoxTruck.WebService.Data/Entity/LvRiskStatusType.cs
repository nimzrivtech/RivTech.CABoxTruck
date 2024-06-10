using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvRiskStatusType : BaseEntity<byte>, IDescription, IIsActive
    {
        public LvRiskStatusType()
        {
            this.IsActive = true;
        }

        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
