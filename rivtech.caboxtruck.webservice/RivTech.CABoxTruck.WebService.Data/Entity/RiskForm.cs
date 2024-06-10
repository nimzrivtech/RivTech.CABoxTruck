using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskForm : BaseEntity<Guid>, ICreatedData
    {
        public RiskForm()
        {
            CreatedDate = DateTime.Now;
        }

        public Guid? RiskDetailId { get; set; }
        public string FormId { get; set; }
        public bool? IsSelected { get; set; }
        public string Other { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public RiskDetail RiskDetail { get; set; }
        public Form Form { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }

        public void Exclude()
        {
            IsActive = false;
            IsSelected = null;
            Other = null;
        }

        public void IncludeWithData(string data)
        {
            IsActive = true;
            Other = data;
        }
    }
}