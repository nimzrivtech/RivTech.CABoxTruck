using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class VehicleUpdateChangeItem : IEndorsementChangeItem
    {
        public  string Type => nameof(VehicleUpdateChangeItem);

        public  string DisplayText => "";

        public Guid? Id { get; }

        public short? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string RegisteredState { get; set; }
        public string Style { get; set; }
        public string VehicleType { get; set; }
        public short? GrossVehicleWeightId { get; set; }
        public short? VehicleDescriptionId { get; set; }
        public short? VehicleBusinessClassId { get; set; }
        public bool? IsAutoLiability { get; set; }
        public bool? IsCoverageFireTheft { get; set; }
        public bool? IsCoverageCollision { get; set; }
        public short? CoverageFireTheftDeductibleId { get; set; }
        public short? CoverageCollisionDeductibleId { get; set; }
        public decimal? StatedAmount { get; set; }
        public bool? HasCoverageCargo { get; set; }
        public bool? HasCoverageRefrigeration { get; set; }
        public short? UseClassId { get; set; }
        public decimal? Radius { get; set; }
        public Guid? GaragingAddressId { get; set; }
        public string PrimaryOperatingZipCode { get; set; }
        public string State { get; set; }
        public decimal? RadiusTerritory { get; set; }
        public Guid MainId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public bool? IsEndorsement1CargoChange { get; set; }
        public bool? IsEndorsement1ReeferChange { get; set; }
    }
}
