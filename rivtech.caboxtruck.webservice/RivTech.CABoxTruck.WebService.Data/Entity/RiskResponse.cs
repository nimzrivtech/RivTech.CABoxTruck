using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskResponse : BaseEntity<Guid>
    {
        public Guid RiskDetailId { get; set; }
        public short QuestionId { get; set; }
        public string ResponseValue { get; set; }
        public Question Question { get; set; }
    }
}