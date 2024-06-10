using System;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class UpdatePolicyHistoryIdRequestDTO
    {
        public Guid RiskId { get; set; }
        public long PolicyHistoryId { get; set; }
    }
}
