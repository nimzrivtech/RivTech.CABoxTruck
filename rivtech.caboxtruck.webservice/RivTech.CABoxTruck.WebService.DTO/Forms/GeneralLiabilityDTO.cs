using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class GeneralLiabilityDTO : IDialogUpdate
    {
        public decimal InjuryLimit { get; set; }
        public decimal MedicalExpenses { get; set; }
        public decimal DamagesRentedToYou { get; set; }
        public decimal AggregateLimit { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}