using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Vehicles
{
    public class VehicleHeaderDTO
    {
        public Guid RiskDetailId { get; set; }
        public int? OptionNumber { get; set; }
        public short? TrailerTypeLimitId { get; set; }
        public short? MainUseId { get; set; }
        public short? MainGVWId { get; set; }
        public short? SecondaryBusinessGroupId { get; set; }
        public short? AccountCategoryId { get; set; }
        public short? AccountCategoryUWId { get; set; }
    }
}
