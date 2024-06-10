using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskNotes : BaseEntity<Guid>
    {
        public RiskNotes()
        {
            CreatedDate = DateTime.Now;
        }
        public Guid RiskDetailId { get; set; }
        public short CategoryId { get; set; }
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string SourcePage { get; set; }

        public RiskDetail RiskDetail { get; set; }
        public NoteCategoryOption NoteCategoryOption { get; set; }
    }
}
