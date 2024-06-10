using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskFilingDTO
    {
        public Guid RiskDetailId { get; set; }
        public int FilingTypeId { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}