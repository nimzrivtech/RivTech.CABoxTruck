
using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RelatedEntityDTO
    {
        public Int64 RelatedEntityID { get; set; }
        public Int64 RiskDetailID { get; set; }
        public Int16 RelatedEntityTypeID { get; set; }
        public Int64 EntityID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ProcessDate { get; set; }
        public decimal EndorsementNumber { get; set; }
    }
}
