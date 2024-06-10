using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class RiskSpecificCommoditiesHauledDto
    {
        public Guid? Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public string Commodity { get; set; }
        public decimal PercentageOfCarry { get; set; }
    }
}
