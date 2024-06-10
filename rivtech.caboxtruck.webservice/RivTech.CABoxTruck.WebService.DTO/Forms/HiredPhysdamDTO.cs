using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class HiredPhysdamDTO : IDialogUpdate
    {
        public decimal EstimatedAdditionalPremium { get; set; }
        public int VehicleDays { get; set; }
        public decimal RatePerDay { get; set; }
        public int GrossCombinedWeight { get; set; }
        public decimal AccidentLimitPerVehicle { get; set; }
        public decimal Deductible { get; set; }
        public decimal PhysdamLimit { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}