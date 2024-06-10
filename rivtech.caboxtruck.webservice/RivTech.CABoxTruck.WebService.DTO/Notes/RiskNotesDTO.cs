using System;

namespace RivTech.CABoxTruck.WebService.DTO.Notes
{
    public class RiskNotesDTO
    {
        public Guid? Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public short CategoryId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string SourcePage { get; set; }

        public NoteCategoryOptionDTO NoteCategoryOption { get; set; }
    }

    public class NoteCategoryOptionDTO
    {
        public short Id { get; set; }
        public string Description { get; set; }
    }
}
