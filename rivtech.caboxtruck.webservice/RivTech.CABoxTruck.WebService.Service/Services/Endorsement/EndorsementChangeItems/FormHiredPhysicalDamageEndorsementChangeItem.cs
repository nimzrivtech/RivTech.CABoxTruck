using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormHiredPhysicalDamageEndorsementChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormHiredPhysicalDamageEndorsementChangeItem);

        public string DisplayText => "";

        public decimal? EstimatedAdditionalPremium { get; set; }
        public int? VehicleDays { get; set; }
        public decimal? RatePerDay { get; set; }
        public int? GrossCombinedWeight { get; set; }
        public decimal? AccidentLimitPerVehicle { get; set; }
        public decimal? Deductible { get; set; }
        public decimal? PhysdamLimit { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
