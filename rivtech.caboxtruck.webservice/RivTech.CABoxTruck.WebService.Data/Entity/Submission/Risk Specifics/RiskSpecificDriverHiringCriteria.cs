using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics
{
    public class RiskSpecificDriverHiringCriteria : BaseEntity<Guid>
    {
        public RiskSpecificDriverHiringCriteria()
        {
            this.CreatedDate = DateTime.Now;
        }
        public RiskDetail Risk { get; set; }
        public Guid Id { get; set; }
        public bool AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years { get; set; }
        public bool DoDriversHave5YearsDrivingExperience { get; set; }
        public bool IsAgreedToReportAllDriversToRivington { get; set; }
        public bool AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto { get; set; }
        public bool AreDriversProperlyLicensedDotCompliant { get; set; }
        public bool IsDisciplinaryPlanDocumented { get; set; }
        public bool IsThereDriverIncentiveProgram { get; set; }
        public List<BackgroundCheckOption> BackGroundCheckIncludes { get; set; }
        public List<DriverTrainingOption> DriverTrainingIncludes { get; set; }

        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
