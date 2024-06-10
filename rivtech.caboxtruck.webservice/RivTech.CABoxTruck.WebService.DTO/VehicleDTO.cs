using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Vehicles;
using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class VehicleDTO
    {
        public Guid? Id { get; set; }
        public Guid? RiskDetailId { get; set; }
        public int? EndorsementNumber { get; set; }
        public Guid MainId { get; set; }
        public bool? isEndorsement { get; set; } = null;
        public short? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string Style { get; set; }
        public string VehicleType { get; set; }
        public short? GrossVehicleWeightId { get; set; }
        public short? VehicleDescriptionId { get; set; }
        public string Description { get; set; }
        public short? VehicleBusinessClassId { get; set; }
        public string BusinessClass { get; set; }

        public bool? IsAutoLiability { get; set; }
        public bool? IsCoverageFireTheft { get; set; }
        public short? CoverageFireTheftDeductibleId { get; set; }

        public bool? IsCoverageCollision { get; set; }
        public short? CoverageCollisionDeductibleId { get; set; }

        public decimal StatedAmount { get; set; }
        public bool? HasCoverageCargo { get; set; }
        public bool? HasCoverageRefrigeration { get; set; }
        public short? UseClassId { get; set; }
        public Guid? GaragingAddressId { get; set; }
        public string PrimaryOperatingZipCode { get; set; }
        public string State { get; set; }
        public string RegisteredState { get; set; }
        public string StateFull { get; set; }
        public string Options { get; set; }

        public decimal? Radius { get; set; }
        public decimal? RadiusTerritory { get; set; }

        public decimal? AutoLiabilityPremium { get; set; }
        public decimal? CargoPremium { get; set; }
        public decimal? RefrigerationPremium { get; set; }
        public decimal? PhysicalDamagePremium { get; set; }
        public decimal? ComprehensivePremium { get; set; }
        public decimal? CollisionPremium { get; set; }
        public decimal? TotalPremium { get; set; }
        public decimal? ProratedPremium { get; set; }

        public string PolicyNumber { get; set; }

        //Excel file upload mapper
        public string GrossVehicleWeight { get; set; }
        public string CoverageCollisionDeductible { get; set; }
        public string CoverageFireTheft { get; set; }
        public string CoverageCollision { get; set; }
        public string CoverageCargo { get; set; }
        public string CoverageRefrigeration { get; set; }
        public string CoverageFireTheftDesc { get; set; }
        public Guid? PreviousVehicleVersionId { get; set; }
        public Int16? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool? IsValidated { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? CanReinstate { get; set; }
        public VehiclePremium LatestVehiclePremium { get; set; }
        public bool? hasPreviousVersions { get; set; }
        public bool? IsManualCoverageFtLimit { get; set; }
        public bool? IsManualCollisionLimit { get; set; }

        public override string ToString()
        {
            return $"{VIN} ({Id.ToString().Substring(0, 5)})";
        }

        public void ClearCollisionCoverage()
        {
            IsCoverageCollision = false;
            CoverageCollisionDeductibleId = null;
            IsManualCollisionLimit = null;
        }

        public void ClearCompFtCoverage()
        {
            IsCoverageFireTheft = false;
            CoverageFireTheftDeductibleId = null;
            IsManualCoverageFtLimit = null;
        }

        public void SetCollisionDedutible(int? collDeductibleId)
        {
            IsCoverageCollision = true;
            CoverageCollisionDeductibleId = (short) collDeductibleId;
        }

        public void SetCompFtDeductible(int? compFTDeductibleId)
        {
            IsCoverageFireTheft = true;
            CoverageFireTheftDeductibleId = (short) compFTDeductibleId;
        }

        public bool HasCompFtLimit()
        {
            return CoverageFireTheftDeductibleId != null
                && CoverageFireTheftDeductibleId != CompFtLimits.NoCov;
        }

        public bool HasCollisionLimit()
        {
            return CoverageCollisionDeductibleId != null
                && CoverageCollisionDeductibleId != CollisionLimits.NoCov;
        }

    }
}
