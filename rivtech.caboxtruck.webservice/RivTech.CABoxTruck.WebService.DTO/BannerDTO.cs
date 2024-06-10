using System;
namespace RivTech.CABoxTruck.WebService.DTO
{
    public class BannerDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string PageLink { get; set; }
        public int? DisplayOrder { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
