using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class SymbolTenResponse : BaseEntity<int>, IIsActive
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
