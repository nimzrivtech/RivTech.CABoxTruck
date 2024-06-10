using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class ProgramDTO
    {
        public Guid InsuranceCompanyId { get; set; }
        public byte ProgramTypeId { get; set; }
        public byte RiskTypeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }   
}

