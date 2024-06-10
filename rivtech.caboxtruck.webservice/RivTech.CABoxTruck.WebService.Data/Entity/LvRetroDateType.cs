using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvRetroDateType : BaseEntity<byte>, IDescription, IIsActive
    {
        public LvRetroDateType()
        {
            this.IsActive = true;
        }

        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
