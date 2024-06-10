using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class CargoChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(CargoChangeItem);

        public string DisplayText => "";
        public bool? AreRequiredToCarryCargoInsurance { get; set; }
        public bool? RequireRefrigerationBreakdownCoverage { get; set; }
        public bool? HaulHazardousMaterials { get; set; }
        public bool? HaveRefrigeratedUnits { get; set; }
        public bool? AreCommoditiesStoredInTruckOvernight { get; set; }
        public string AreCommoditiesStoredInTruckOvernightExplain { get; set; }
        public bool? HadCargoLoss { get; set; }
        public string HadCargoLossExplain { get; set; }
    }
}
