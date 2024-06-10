using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Vehicle : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public Guid MainId { get; set; }
        public Guid RiskDetailId => RiskDetailVehicles
            ?.Select(rdv => rdv.RiskDetail)
            ?.OrderByDescending(rdv => rdv?.CreatedDate)
            ?.FirstOrDefault()?.Id ?? Guid.Empty;

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
        public short? CoverageFireTheftDeductibleId { get; set; }
        public bool? IsCoverageCollision { get; set; }
        public short? CoverageCollisionDeductibleId { get; set; }
        public decimal StatedAmount { get; set; }
        public bool? HasCoverageCargo { get; set; }
        public bool? HasCoverageRefrigeration { get; set; }
        public short? UseClassId { get; set; }
        public decimal? Radius { get; set; }
        public Guid? GaragingAddressId { get; set; }
        public string PrimaryOperatingZipCode { get; set; }
        public string State { get; set; }
        public decimal? RadiusTerritory { get; set; }
        public string Options { get; set; }
        public decimal? AutoLiabilityPremium { get; set; }
        public decimal? CargoPremium { get; set; }
        public decimal? RefrigerationPremium { get; set; }
        public decimal? PhysicalDamagePremium { get; set; }
        public decimal? ComprehensivePremium { get; set; }
        public decimal? CollisionPremium { get; set; }
        public decimal? TotalPremium { get; set; }
        public bool? IsManualCoverageFtLimit { get; set; }
        public bool? IsManualCollisionLimit { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public RiskDetail RiskDetail => RiskDetailVehicles?.Select(rdv => rdv?.RiskDetail)
            ?.OrderByDescending(x => x?.UpdatedDate)
            ?.FirstOrDefault();
        public RiskDetail LatestRiskDetail => RiskDetailVehicles?.Select(rdv => rdv?.RiskDetail)
            ?.OrderByDescending(x => x?.CreatedDate)
            ?.FirstOrDefault();

        public RiskVehiclePremiumTransaction RiskVehiclePremiumTransaction { get; set; }

        public bool? IsActive { get; set; }
        /// <summary>
        /// The date when the vehicle was actually deleted. Refer to ExpirationDate for its effective deleted date.
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        public bool? IsValidated { get; set; }

        public virtual ICollection<RiskDetailVehicle> RiskDetailVehicles { get; set; } = new List<RiskDetailVehicle>();
        public Vehicle PreviousVehicleVersion { get; set; }
        public Guid? PreviousVehicleVersionId { get; set; } = null;
        public bool IsDeleted => DeletedDate != null;

        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public int EndorsementNumber { get; set; } // Only used for viewing of previous vehicle in UI.
        //public Guid? VehicleProratedPremiumId { get; set; }
        //public VehicleProratedPremium VehicleProratedPremium { get; set; }
        public IEnumerable<VehiclePremium> VehiclePremiums { get; set; }
        public VehiclePremium LatestVehiclePremium => VehiclePremiums?.OrderByDescending(x => x.CreatedDate)?.FirstOrDefault();
        public VehiclePremiumRatingFactor VehiclePremiumRatingFactor { get; set; }

        /// <summary>
        /// Links vehicle to a RiskDetail.
        /// </summary>
        /// <param name="riskDetail"></param>
        public void LinkRiskDetail(RiskDetail riskDetail)
        {
            RiskDetailVehicles ??= new List<RiskDetailVehicle>();
            RiskDetailVehicles.Add(new RiskDetailVehicle(riskDetail, this));
        }

        /// <summary>
        /// Returns a new version of this vehicle.
        /// </summary>
        /// <returns></returns>
        public Vehicle CloneForEndorsement(bool linkToLatestRiskDetail = true)
        {
            Vehicle newVehicleVersion = (Vehicle)MemberwiseClone();
            newVehicleVersion.Id = Guid.NewGuid();
            newVehicleVersion.PreviousVehicleVersionId = Id;
            newVehicleVersion.PreviousVehicleVersion = null;
            newVehicleVersion.CreatedDate = CreatedDate;
            newVehicleVersion.UpdatedDate = DateTime.Now;
            newVehicleVersion.ClearRiskDetailVehicles();

            if (linkToLatestRiskDetail)
                newVehicleVersion.LinkRiskDetail(LatestRiskDetail);

            return newVehicleVersion;
        }

        public Vehicle CloneForRenewal()
        {
            var clone = (Vehicle)MemberwiseClone();
            clone.Id = Guid.NewGuid();
            clone.PreviousVehicleVersionId = null;
            clone.PreviousVehicleVersion = null;
            clone.CreatedDate = DateTime.Now;
            clone.UpdatedDate = DateTime.Now;
            clone.DeletedDate = null;
            clone.ClearRiskDetailVehicles();
            clone.Radius ??= 50;
            clone.ClearPremiums();
            clone.Options = "1";
            return clone;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vehicle;
            return other != null && other.Id.Equals(this.Id);
        }

        public void Delete(DateTime? effectiveDate = null)
        {
            IsActive = false;
            DeletedDate = DateTime.Now;
            ExpirationDate = effectiveDate;
        }

        public void ClearRiskDetailVehicles()
        {
            RiskDetailVehicles = new List<RiskDetailVehicle>();
        }

        public RiskDetailVehicle UnlinkLatestRiskDetail()
        {
            var rdv = RiskDetailVehicles.FirstOrDefault(rdv => rdv.RiskDetail.Equals(LatestRiskDetail));
            if (rdv != null)
            {
                RiskDetailVehicles = RiskDetailVehicles.Where(x => x != rdv).ToList();
            }
            return rdv;
        }

        public void UpdateLimits(RiskCoverage riskCoverage)
        {
            bool hasNoComprehensiveDeductible = (riskCoverage.CompDeductibleId ?? LvLimits.ComprehensiveNoCoverageId) == LvLimits.ComprehensiveNoCoverageId;
            bool hasNoFireAndTheftDeductible = (riskCoverage.FireDeductibleId ?? LvLimits.ComprehensiveNoCoverageId) == LvLimits.ComprehensiveNoCoverageId;
            bool hasNoCollisionDeductible = (riskCoverage.CollDeductibleId ?? LvLimits.ComprehensiveNoCoverageId) == LvLimits.CollisionNoCoverageId;
            bool hasNoCargoLimits = (riskCoverage.CargoLimitId ?? LvLimits.CargoNoneCoverageId) == LvLimits.CargoNoneCoverageId;
            bool hasNoRefrigerationLimits = (riskCoverage.RefCargoLimitId ?? LvLimits.RefNoneCoverageId) == LvLimits.RefNoneCoverageId;
            bool isOption1 = riskCoverage.OptionNumber == 1;

            // Update comp/ft limit.
            if (isOption1)
            {
                if (hasNoComprehensiveDeductible && hasNoFireAndTheftDeductible)
                {
                    CoverageFireTheftDeductibleId = null;
                    IsCoverageFireTheft = false;
                }
            }
            else
            {
                IsCoverageFireTheft = true;
                CoverageFireTheftDeductibleId = riskCoverage.FireDeductibleId.HasValue
                    ? riskCoverage.FireDeductibleId
                    : riskCoverage.CompDeductibleId;
            }

            // Updated coll limits.
            if (isOption1)
            {

                if (hasNoCollisionDeductible)
                {
                    CoverageCollisionDeductibleId = null;
                    IsCoverageCollision = false;
                }
            }
            else
            {
                CoverageCollisionDeductibleId = riskCoverage.CollDeductibleId;
                IsCoverageCollision = true;
            }

            if (isOption1)
            {
                if (hasNoCargoLimits) HasCoverageCargo = false;
            }
            else
            {
                HasCoverageCargo = !hasNoCargoLimits;
            }

            if (isOption1)
            {
                if (hasNoRefrigerationLimits) HasCoverageRefrigeration = false;
            }
            else
            {
                HasCoverageRefrigeration = !hasNoRefrigerationLimits;
            }
        }

        public void Reinstate()
        {
            DeletedDate = null;
            IsActive = true;
        }

        /// <summary>
        /// Checks if vehicle has to create/rollback version.
        /// </summary>
        public bool HasChanges(Vehicle other)
        {
            if (other is null)
                return false;

            return false
                || EffectiveDate != other.EffectiveDate
                || ExpirationDate != other.ExpirationDate
                || CoverageCollisionDeductibleId != other.CoverageCollisionDeductibleId
                || CoverageFireTheftDeductibleId != other.CoverageFireTheftDeductibleId
                || GaragingAddressId != other.GaragingAddressId
                || GrossVehicleWeightId != other.GrossVehicleWeightId
                || IsCoverageCollision != other.IsCoverageCollision
                || IsCoverageFireTheft != other.IsCoverageFireTheft
                || HasCoverageCargo != other.HasCoverageCargo
                || HasCoverageRefrigeration != other.HasCoverageRefrigeration
                || Make != other.Make
                || Model != other.Model
                || PrimaryOperatingZipCode != other.PrimaryOperatingZipCode
                || Radius != other.Radius
                || RadiusTerritory != other.RadiusTerritory
                || RegisteredState != other.RegisteredState
                || StatedAmount != other.StatedAmount
                || Style != other.Style
                || UseClassId != other.UseClassId
                || VehicleBusinessClassId != other.VehicleBusinessClassId
                || VehicleDescriptionId != other.VehicleDescriptionId
                || VehicleType != other.VehicleType
                || VIN != other.VIN
                || Year != other.Year
                || Options != other.Options
                || IsValidated != other.IsValidated
                || IsManualCoverageFtLimit != other.IsManualCoverageFtLimit
                || IsManualCollisionLimit != other.IsManualCollisionLimit;
        }

        public bool IsEffectiveExpirationDateChanged(Vehicle other)
        {
            if (other is null)
                return false;

            return false
                || EffectiveDate != other.EffectiveDate
                || ExpirationDate != other.ExpirationDate;
        }

        public bool IsIncludedInOption(int option)
        {
            return !string.IsNullOrEmpty(Options) ? Options.Contains(option.ToString()) : false;
        }

        public void UpdateCoveragesFromOption(RiskCoverage riskCoverage)
        {
            SetCompFtDeductible(riskCoverage.CompFTDeductibleId);
            SetCollisionDeductible(riskCoverage.CollDeductibleId);
        }

        public void SetCompFtDeductible(int? compFtDeductibleId)
        {
            if (compFtDeductibleId is null || compFtDeductibleId == LvLimits.ComprehensiveNoCoverageId)
            {
                IsCoverageFireTheft = false;
                CoverageFireTheftDeductibleId = null;
            }
            else
            {
                IsCoverageFireTheft = true;
                CoverageFireTheftDeductibleId = (short)compFtDeductibleId;
            }
        }

        public void SetCollisionDeductible(int? collisionDeductibleId)
        {
            if (collisionDeductibleId is null || collisionDeductibleId == LvLimits.CollisionNoCoverageId)
            {
                IsCoverageCollision = false;
                CoverageCollisionDeductibleId = null;
            }
            else
            {
                IsCoverageCollision = true;
                CoverageCollisionDeductibleId = (short)collisionDeductibleId;
            }
        }

        public bool HasCompFtLimit()
        {
            return CoverageFireTheftDeductibleId != null
                && CoverageFireTheftDeductibleId != LvLimits.ComprehensiveNoCoverageId;
        }

        public bool HasCollisionLimit()
        {
            return CoverageCollisionDeductibleId != null
                && CoverageFireTheftDeductibleId != LvLimits.CollisionNoCoverageId;
        }

        public void SetCompAndColIsManual(Vehicle savedVehicle, RiskCoverage riskCoverage, bool isNew = false)
        {
            if (isNew)
            {
                if (riskCoverage.CompFTDeductibleId != LvLimits.ComprehensiveNoCoverageId &&
                    riskCoverage.CompFTDeductibleId != CoverageFireTheftDeductibleId)
                {
                    IsManualCoverageFtLimit = true;
                }

                if (riskCoverage.CollDeductibleId != LvLimits.CollisionNoCoverageId &&
                    riskCoverage.CollDeductibleId != CoverageCollisionDeductibleId)
                {
                    IsManualCollisionLimit = true;
                }
            }
            else
            {
                if (riskCoverage.CompFTDeductibleId != LvLimits.ComprehensiveNoCoverageId)
                {
                    if (savedVehicle?.CoverageFireTheftDeductibleId != CoverageFireTheftDeductibleId &&
                        savedVehicle?.CoverageFireTheftDeductibleId != null && riskCoverage.CompFTDeductibleId != CoverageFireTheftDeductibleId)
                        IsManualCoverageFtLimit = true;
                }

                if (riskCoverage.CollDeductibleId != LvLimits.CollisionNoCoverageId)
                {
                    if (savedVehicle?.CoverageCollisionDeductibleId != CoverageCollisionDeductibleId &&
                        savedVehicle?.CoverageCollisionDeductibleId != null && riskCoverage.CollDeductibleId != CoverageCollisionDeductibleId)
                        IsManualCollisionLimit = true;
                }
            }

        }

        public void ClearPremiums()
        {
            AutoLiabilityPremium = null;
            PhysicalDamagePremium = null;
            CargoPremium = null;
            CollisionPremium = null;
            ComprehensivePremium = null;
            RefrigerationPremium = null;
            TotalPremium = null;
            RadiusTerritory = null;
            Radius ??= 50;
        }
    }
}
