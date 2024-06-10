using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class DriverDTO
    {
        public Guid? Id { get; set; }
        public Guid? RiskDetailId { get; set; }
        public Guid? EntityId { get; set; }

        public DateTime? BirthDate { get; set; }
        public string Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public short? YrsDrivingExp { get; set; }
        public short? YrsCommDrivingExp { get; set; }

        public string State { get; set; }
        public string StateFull { get; set; }
        public string LicenseNumber { get; set; }
        
        public bool? IsMvr { get; set; }

        public string YearPoint { get; set; }
        public string AgeFactor { get; set; }
        public string TotalFactor { get; set; }

        public bool? IsOutOfState { get; set; }

        public DateTime? MvrDate { get; set; }
        public DateTime? HireDate { get; set; }

        public bool? isExcludeKO { get; set; }
        public string koReason { get; set; }

        public string Options { get; set; }

        public string PolicyNumber { get; set; }
        public string SubmissionNumber { get; set; }

        public short? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool? hasError { get; set; }

        public bool? IsValidated { get; set; }
        public bool? IsValidatedKO { get; set; }
        public bool? canReinstate { get; set; }
        //public EntityDTO Entity { get; set; }
        public List<DriverIncidentsDTO> DriverIncidents { get; set; }

        public Guid? PreviousDriverVersionId { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class DriverIncidentsDTO
    {
        public Guid? Id { get; set; }
        public Guid? DriverId { get; set; }
        public string IncidentType { get; set; }
        public DateTime? IncidentDate { get; set; }
        public DateTime? ConvictionDate { get; set; }
        public Guid? UniqueId { get; set; }
    }

    public class DriverHeaderDTO
    {
        public Guid? Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public bool? driverRatingTab { get; set; }
        public bool? accidentInfo { get; set; }
        public DateTime? mvrHeaderDate { get; set; }
        public string applyFilter { get; set; }
        public string accDriverFactor { get; set; }
        public string driverVehicleRatio { get; set; }
    }
}
