using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class TrailerInterchangeDTO : IDialogUpdate
    {
        public decimal CompreLimitOfInsurance { get; set; }
        public decimal CompreDeductible { get; set; }
        public decimal ComprePremium { get; set; }

        public decimal LossLimitOfInsurance { get; set; }
        public decimal LossDeductible { get; set; }
        public decimal LossPremium { get; set; }

        public decimal CollisionLimitOfInsurance { get; set; }
        public decimal CollisionDeductible { get; set; }
        public decimal CollisionPremium { get; set; }

        public decimal FireLimitOfInsurance { get; set; }
        public decimal FireDeductible { get; set; }
        public decimal FirePremium { get; set; }

        public decimal FireTheftLimitOfInsurance { get; set; }
        public decimal FireTheftDeductible { get; set; }
        public decimal FireTheftPremium { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}