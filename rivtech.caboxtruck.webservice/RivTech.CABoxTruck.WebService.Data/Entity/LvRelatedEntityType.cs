using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvRelatedEntityType : BaseEntity<byte>, IDescription, IIsActive
    {
        public LvRelatedEntityType()
        {
            this.IsActive = true;
        }

        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
