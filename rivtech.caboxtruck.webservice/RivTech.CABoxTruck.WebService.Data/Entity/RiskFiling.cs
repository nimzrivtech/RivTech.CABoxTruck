using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskFiling : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public Guid RiskDetailId { get; set; }
        public int FilingTypeId { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public RiskDetail RiskDetail { get; set; }
    }
}