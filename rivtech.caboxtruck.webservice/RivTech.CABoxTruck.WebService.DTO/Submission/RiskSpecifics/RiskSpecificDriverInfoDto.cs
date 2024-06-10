using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class RiskSpecificDriverInfoDto
    {
        public Guid? Id { get; set; }
        public int? TotalNumberOfDrivers { get; set; }
        public int DriversHired { get; set; }
        public int DriversTerminated { get; set; }
        public EnumerationDTO MvrPullingFrequency { get; set; }
        public bool IsBusinessPrincipalADriverInPolicy { get; set; }
        public bool AreVolunteersUsedInBusiness { get; set; }
        public decimal? PercentageOfStaff { get; set; }
        public bool IsHiringFromOthers { get; set; }
        public decimal? AnnualCostOfHire { get; set; }
        public bool IsHiringFromOthersWithoutDriver { get; set; }
        public decimal? AnnualCostOfHireWithoutDriver { get; set; }
        public bool IsLeasingToOthers { get; set; }
        public decimal? AnnualIncomeDerivedFromLease { get; set; }
        public bool IsLeasingToOthersWithoutDriver { get; set; }
        public decimal? AnnualIncomeDerivedFromLeaseWithoutDriver { get; set; }
        public bool IsThereAssumedLiabilityByContract { get; set; }
        public bool DoDriversTakeVehiclesHome { get; set; }
        public bool AreVehiclesSolelyOwnedByApplicant { get; set; }
        public bool WillBeAddingDeletingVehiclesDuringTerm { get; set; }
        public string AddingDeletingVehiclesDuringTermDescription { get; set; }
    }
}
