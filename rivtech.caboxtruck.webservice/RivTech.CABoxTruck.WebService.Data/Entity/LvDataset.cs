using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvDataset : BaseEntity<int>, IIsActive
    {
        public LvDataset() {
            this.IsActive = true;
        }
        public int Value { get; set; }
        public string Description { get; set; }
        public string DescriptionInUI { get; set; }
        public int OrderInUI { get; set; }
        public string SupportedInRaterVersion { get; set; }
        public string DatasetType { get; set; }
        public bool IsActive { get; set; }
    }
}
