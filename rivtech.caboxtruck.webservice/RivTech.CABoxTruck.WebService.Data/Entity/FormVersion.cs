using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class FormVersion : BaseEntity<string>
    {
        public string FormId { get; set; }
        public int Version { get; set; }
        public string FormNumber { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
