using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvRiskType : BaseEntity<byte>
    {
        public LvRiskType()
        {
            this.IsActive = true;
        }

        public string Name { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
