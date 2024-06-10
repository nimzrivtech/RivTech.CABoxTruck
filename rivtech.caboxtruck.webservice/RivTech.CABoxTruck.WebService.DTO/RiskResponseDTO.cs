using RivTech.CABoxTruck.WebService.Data.Entity;
using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskResponseDTO
    {
        public Guid Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public Int16 QuestionId { get; set; }
        public string ResponseValue { get; set; }

        public Question Question { get; set; }
    }
}
