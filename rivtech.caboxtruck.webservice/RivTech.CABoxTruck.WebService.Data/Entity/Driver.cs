using RivTech.CABoxTruck.WebService.Data.Entity.Drivers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Driver : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public Driver()
        {
            Id = Guid.NewGuid();
            DriverIncidents = new List<DriverIncidents>();
        }

        //public Guid RiskDetailId => RiskDetailDrivers
        //   ?.Select(rdv => rdv.RiskDetail)
        //   ?.OrderByDescending(rdv => rdv.CreatedDate)
        //   ?.FirstOrDefault()?.Id ?? Guid.Empty;
        public Guid EntityId { get; set; }
        public string LicenseNumber { get; set; }
        public string State { get; set; }
        public bool? IsOutOfState { get; set; }
        public bool? IsMvr { get; set; }
        public DateTime? MvrDate { get; set; }
        public DateTime? HireDate { get; set; }
        public short? YrsDrivingExp { get; set; }
        public short? YrsCommDrivingExp { get; set; }
        public bool? isExcludeKO { get; set; }
        public string koReason { get; set; }
        public string TotalFactor { get; set; }
        public string YearPoint { get; set; }
        public string AgeFactor { get; set; }
        public string Options { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsValidatedKO { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public Entity Entity { get; set; }
        public List<DriverIncidents> DriverIncidents { get; set; }
        public List<RiskDetailDriver> RiskDetailDrivers { get; set; }
        public Driver PreviousDriverVersion { get; set; }
        public Guid? PreviousDriverVersionId { get; set; } = null;
        public bool IsDeleted => DeletedDate != null;

        public bool IsSameWith(Driver otherDriver)
        {
            return otherDriver != null
                && LicenseNumber == otherDriver.LicenseNumber
                && State == otherDriver.State;
        }

        public void LinkRiskDetail(RiskDetail riskDetail)
        {
            RiskDetailDrivers ??= new List<RiskDetailDriver>();
            RiskDetailDrivers.Add(new RiskDetailDriver(riskDetail, this));
        }

        //public RiskDetail RiskDetail => RiskDetailDrivers?.Select(rdv => rdv.RiskDetail)
        //   ?.OrderByDescending(x => x.UpdatedDate)
        //   ?.FirstOrDefault();

        //public Driver CreateNewVersionForEndorsement()
        //{
        //   Driver newDriverVersion = (Driver)MemberwiseClone();
        //   newDriverVersion.Id = Guid.NewGuid();
        //   newDriverVersion.PreviousDriverVersion = this;
        //   return newDriverVersion;
        //}

        public void ClearRiskDetailDrivers()
        {
            RiskDetailDrivers = new List<RiskDetailDriver>();
        }

        public RiskDetailDriver UnlinkLatestRiskDetail()
        {
            var RiskDetail = RiskDetailDrivers?.OrderByDescending(x => x.UpdatedDate)
            ?.Select(rdv => rdv.RiskDetail)
            ?.FirstOrDefault();
            var rdv = RiskDetailDrivers.FirstOrDefault(rdv => rdv.RiskDetail.Equals(RiskDetail));
            if (rdv != null)
            {
                RiskDetailDrivers.Remove(rdv);
            }
            return rdv;
        }

        public Driver CloneForEndorsement()
        {
            var RiskDetail = RiskDetailDrivers?.OrderByDescending(x => x.UpdatedDate)
            ?.Select(rdv => rdv.RiskDetail)
            ?.FirstOrDefault();
            Driver newDriverVersion = (Driver)MemberwiseClone();
            newDriverVersion.Id = Guid.NewGuid();
            newDriverVersion.PreviousDriverVersionId = Id;
            newDriverVersion.PreviousDriverVersion = null;
            newDriverVersion.CreatedDate = CreatedDate;
            newDriverVersion.UpdatedDate = DateTime.Now;
            newDriverVersion.ClearRiskDetailDrivers();
            newDriverVersion.LinkRiskDetail(RiskDetail);
            newDriverVersion.Entity = null;
            newDriverVersion.DriverIncidents = null;
            return newDriverVersion;
        }
        public void Delete(DateTime? effectiveDate = null)
        {
            IsActive = false;
            DeletedDate = DateTime.Now;
            ExpirationDate = effectiveDate;
        }

        public bool HasOptions()
        {
            return Options?.Split(",").Any(x => int.TryParse(x, out _)) ?? false;
        }

        public void ClearFactors()
        {
            AgeFactor = null;
            TotalFactor = null;
            YearPoint = null;
        }

        public Driver CloneForRenewal()
        {
            var clonedDriver = (Driver)MemberwiseClone();
            clonedDriver.Id = Guid.Empty;
            clonedDriver.ClearFactors();
            clonedDriver.ClearRiskDetailDrivers();
            clonedDriver.DeletedDate = null;
            clonedDriver.IsActive = null;
            clonedDriver.Options = "1";
            clonedDriver.Entity = Entity.CloneWithNewId();

            clonedDriver.DriverIncidents = new List<DriverIncidents>();
            foreach (var incident in DriverIncidents)
            {
                clonedDriver.DriverIncidents.Add(incident.Clone());
            }

            return clonedDriver;
        }

        public bool IsEffectiveExpirationDateChanged(Driver other)
        {
            if (other is null)
                return false;

            return false
                || EffectiveDate != other.EffectiveDate
                || ExpirationDate != other.ExpirationDate;
        }
    }

    public class DriverIncidents : BaseEntity<Guid>
    {
        public DriverIncidents()
        {
            Id = Guid.NewGuid();
        }

        public Guid DriverId { get; set; }
        public string IncidentType { get; set; }
        public DateTime? IncidentDate { get; set; }
        public DateTime? ConvictionDate { get; set; }
        public Guid UniqueId { get; set; }

        public DriverIncidents Clone()
        {
            var clone = (DriverIncidents)MemberwiseClone();
            clone.Id = Guid.Empty;
            return clone;
        }
    }

    public class DriverHeader : BaseEntity<Guid>
    {
        public DriverHeader()
        {
            Id = Guid.NewGuid();
        }
        public Guid RiskDetailId { get; set; }

        public bool? driverRatingTab { get; set; }
        public bool? accidentInfo { get; set; }
        public DateTime? mvrHeaderDate { get; set; }
        public string applyFilter { get; set; }
        public string accDriverFactor { get; set; }
        public string driverVehicleRatio { get; set; }
    }
}
