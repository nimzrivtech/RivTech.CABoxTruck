using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class RiskSpecificDotInfoDto
    {
        public Guid? Id { get; set; }
        public bool DoesApplicantRequireDotNumber { get; set; }
        public bool DoesApplicantPlanToStayInterstate { get; set; }
        public bool? DoesApplicantHaveInsterstateAuthority { get; set; }
        public DateTime DotRegistrationDate { get; set; }
        public bool IsDotCurrentlyActive { get; set; }
        public EnumerationDTO ChameleonIssues { get; set; }
        public EnumerationDTO CurrentSaferRating { get; set; }
        public EnumerationDTO IssCabRating { get; set; }
        public bool IsThereAnyPolicyLevelAccidents { get; set; }
        public int NumberOfPolicyLevelAccidents { get; set; }
        public bool IsUnsafeDrivingChecked { get; set; }
        public bool IsHoursOfServiceChecked { get; set; }
        public bool IsDriverFitnessChecked { get; set; }
        public bool IsControlledSubstanceChecked { get; set; }
        public bool IsVehicleMaintenanceChecked { get; set; }
        public bool IsCrashChecked { get; set; }
    }
}
