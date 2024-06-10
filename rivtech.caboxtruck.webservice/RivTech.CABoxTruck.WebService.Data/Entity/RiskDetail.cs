using RivTech.CABoxTruck.WebService.Data.Entity.Drivers;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskDetail : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public string SubmissionNumber { get; set; }
        public string QuoteNumber { get; set; }
        public DateTime? FirstIssueDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public DateTime? RequestedQuoteDate { get; set; }
        public string Term { get; set; }
        public short? ProgramID { get; set; }
        public Guid? InsuredId { get; set; }
        public Guid RiskId { get; set; }
        public short? FirstYearUnderCurrentManagement { get; set; }
        public short? AccountCategoryId { get; set; }
        public short? AccountCategoryUWId { get; set; }
        public short? SecondaryBusinessGroupId { get; set; }
        public short? MainUseId { get; set; }
        public short? MainGVWId { get; set; }
        public bool? IsNewVenture { get; set; }
        public short? NewVenturePreviousExperienceId { get; set; }
        public string PreviousExperienceCompany { get; set; }
        public int? PrimaryCityZipCodeOfOperationsId { get; set; }
        public bool? HasOwnedCompanies { get; set; }
        public string PastCompanyDetails { get; set; }
        public short? RiskStateId { get; set; }
        public decimal? AgentCommission { get; set; }
        public bool? IsARatedCarrierRequired { get; set; }
        public bool? IsFilingRequired { get; set; }
        public bool? IsDifferentUSDOTNumberLastFiveYears { get; set; }
        public bool? IsInsuredHasMotorCarrierNo { get; set; }
        public string DOTNumberLastFiveYearsDetail { get; set; }
        public bool? IsOthersAllowedToOperateCurrent { get; set; }
        public bool? IsOthersAllowedToOperatePast { get; set; }
        public bool? IsOperatorsWorkOnlyForInsured { get; set; }
        public string PastOperatingAuthorityStates { get; set; }
        public string CurrentOperatingAuthorityStates { get; set; }
        public bool? IsNewBusiness { get; set; }
        public short? RenewalCount { get; set; }
        public short? YearsInBusiness { get; set; }
        public short? YearsUnderCurrentManagement { get; set; }
        public short? AverageMilesPerVehicleId { get; set; }
        public decimal? DriverMaxHoursDayDriving { get; set; }
        public decimal? DriverMaxHoursDayOnDuty { get; set; }
        public decimal? DriverMaxHoursWeekDriving { get; set; }
        public decimal? DriverMaxHoursWeekOnDuty { get; set; }
        public string HoursOfService { get; set; }
        public string DaysOfService { get; set; }
        public int? ShiftsPerDay { get; set; }
        public int? RadiusOfOperationsPercent1 { get; set; }
        public int? RadiusOfOperationsPercent2 { get; set; }
        public int? RadiusOfOperationsPercent3 { get; set; }
        public bool? HasOperationsOver300MileRadius { get; set; }
        public int? PercentOwnerOperatorVehicles { get; set; }
        public int? NumberOfDrivers { get; set; }
        public bool? IsUWQuestionResponseConfirmed { get; set; }
        public string Status { get; set; }
        public int? AssignedToId { get; set; }
        public bool? IsAdditionalInsured { get; set; }
        public bool? IsWaiverOfSubrogation { get; set; }
        public bool? IsPrimaryNonContributory { get; set; }
        public bool? IsProcessed { get; set; }
        
        public DateTime? EndorsementEffectiveDate { get; set; }
        public Insured Insured { get; set; }
        public int EndorsementNumber { get; set; }
        public Guid? SourceRiskDetailId { get; set; }
        public int VersionNumber { get; set; }


        public RiskSpecificRadiusOfOperation RadiusOfOperations { get; set; }
        public RiskSpecificDriverInfo DriverInfo { get; set; }
        public RiskSpecificDotInfo DotInfo { get; set; }
        public RiskSpecificDriverHiringCriteria DriverHiringCriteria { get; set; }
        public RiskSpecificUnderwritingQuestion UnderwritingQuestions { get; set; }
        public List<RiskFiling> RiskFilings { get; set; }
        public List<Driver> Drivers => RiskDetailDrivers?.Select(rdd => rdd.Driver)?.ToList() ?? new List<Driver>();
        public List<Vehicle> Vehicles { 
            get
            {
                if (RiskDetailVehicles is null)
                    return new List<Vehicle>();
                return RiskDetailVehicles.Select(rdv => rdv.Vehicle).ToList();
            } 
        }
        public DriverHeader DriverHeader { get; set; }
        public List<RiskCoverage> RiskCoverages { get; set; }
        public List<RiskSpecificCommoditiesHauled> CommoditiesHauled { get; set; }
        public List<RiskAdditionalInterest> AdditionalInterests { get; set; }
        public List<RiskResponse> RiskResponses { get; set; }
        public List<FileUploadDocument> FileUploadDocuments { get; set; }
        public List<VehiclePremiumRatingFactor> VehiclePremiumRatingFactors { get; set; }
        public virtual ICollection<RiskDetailVehicle> RiskDetailVehicles { get; set; }


        public void ClearRiskDetailVehicles()
        {
            RiskDetailVehicles = new List<RiskDetailVehicle>();
        }

        public void ClearRiskDetailDrivers()
        {
            RiskDetailDrivers = new List<RiskDetailDriver>();
        }

        public List<RiskDetailDriver> RiskDetailDrivers { get; set; }

        public RiskDetail()
        {
            CommoditiesHauled = new List<RiskSpecificCommoditiesHauled>();
        }

        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool FromReinstatement { get; set; }
        public int? CountPowerUnit { get; set; }
        public int? CountTrailer { get; set; }
        public int? CountOthers { get; set; }

        public RiskDetail DeepClone()
        {
            var clone = (RiskDetail)this.MemberwiseClone();
            clone.CommoditiesHauled = new List<RiskSpecificCommoditiesHauled>();
            foreach(var item in CommoditiesHauled)
            {
                clone.CommoditiesHauled.Add((RiskSpecificCommoditiesHauled)item.Clone());
            }
            return clone;
        }

        public void LinkVehicle(Vehicle clonedVehicle)
        {
            if (RiskDetailVehicles is null)
                RiskDetailVehicles = new List<RiskDetailVehicle>();
            RiskDetailVehicles.Add(new RiskDetailVehicle(this, clonedVehicle));
        }

        public void IncrementEndorsementNumber()
        {
            EndorsementNumber += 1;
        }

        public void SetNewEndorsementNumber(int endorsementNumber)
        {
            EndorsementNumber = endorsementNumber;
            VersionNumber = 0;
        }

    }
}
