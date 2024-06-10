using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics
{
    public class RiskSpecificDotInfo : BaseEntity<Guid>
    {
        public RiskSpecificDotInfo()
        {
            this.CreatedDate = DateTime.Now;
        }

        public RiskDetail RiskDetail { get; set; }
        public bool DoesApplicantRequireDotNumber { get; set; }
        public bool DoesApplicantPlanToStayInterstate { get; set; }
        public bool? DoesApplicantHaveInsterstateAuthority { get; set; }
        public DateTime DotRegistrationDate { get; set; }
        public bool IsDotCurrentlyActive { get; set; }
        public ChameleonIssuesOption ChameleonIssues { get; set; }
        public SaferRatingOption CurrentSaferRating { get; set; }
        public TrafficLightRatingOption IssCabRating { get; set; }
        public bool IsThereAnyPolicyLevelAccidents { get; set; }
        public int NumberOfPolicyLevelAccidents { get; set; }
        public bool IsUnsafeDrivingChecked { get; set; }
        public bool IsHoursOfServiceChecked { get; set; }
        public bool IsDriverFitnessChecked { get; set; }
        public bool IsControlledSubstanceChecked { get; set; }
        public bool IsVehicleMaintenanceChecked { get; set; }
        public bool IsCrashChecked { get; set; }

        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
