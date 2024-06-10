using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class RiskSpecificDriverHiringCriteriaDto
    {
        public Guid? Id { get; set; }
        public bool AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years { get; set; }
        public bool DoDriversHave5YearsDrivingExperience { get; set; }
        public bool IsAgreedToReportAllDriversToRivington { get; set; }
        public bool AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto { get; set; }
        public bool AreDriversProperlyLicensedDotCompliant { get; set; }
        public bool IsDisciplinaryPlanDocumented { get; set; }
        public bool IsThereDriverIncentiveProgram { get; set; }
        public List<EnumerationDTO> BackGroundCheckIncludes { get; set; }
        public List<EnumerationDTO> DriverTrainingIncludes { get; set; }
    }
}
