using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class LvDatasetDTO
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
        public string DescriptionInUI { get; set; }
        public int OrderInUI { get; set; }
        public string SupportedInRaterVersion { get; set; }
        public string DatasetType { get; set; }
        public bool IsActive { get; set; }
    }

    public class LvDatasetListDTO
    {
        public List<LvDatasetListDTO> MainGVWList { get; set; }
        public List<LvDatasetListDTO> VehicleGVWList { get; set; }
        public List<LvDatasetListDTO> VehicleDescList { get; set; }

    }
}
