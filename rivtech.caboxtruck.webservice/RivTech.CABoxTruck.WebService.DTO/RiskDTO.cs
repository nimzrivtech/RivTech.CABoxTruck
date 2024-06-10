using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO.Notes;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.GeneralLiabilityCargo;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskDTO
    {
        public Guid? Id { get; set; }
        public Guid RiskId { get; set; }
        public string SubmissionNumber { get; set; }
        public string QuoteNumber { get; set; }
        public string PolicyNumber { get; set; }
        public int EndorsementNumber { get; set; }
        public string PolicySuffix { get; set; }
        public DateTime? FirstIssueDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? PendingCancellationDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? QuoteNeededBy { get; set; }
        public string Term { get; set; }
        public short? ProgramID { get; set; }
        public Guid? InsuredId { get; set; }
        //public Int32 agencyID { get; set; }
        //public Int32 subAgencyID { get; set; }
        //public Int64 agentID { get; set; }
        public string JurisdictionState { get; set; }
        public decimal? AgentCommission { get; set; }
        public short? PaymentPlanID { get; set; }
        public int? ProductTypeID { get; set; }
        public int? UnderWriterID { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public int? AssignedToId { get; set; }

        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public short? IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsPolicy { get; set; }
        public bool HasInitialDriverExclusion { get; set; }
        public DateTime? PendingCancellationNoticeDate { get; set; }
        public bool IsPendingCancellation { get; set; }
        public IEnumerable<RiskPolicyContactDTO> RiskPolicyContacts { get; set; }

        // add this to populate the source. 
        //enable the include table on repository
        public BrokerInfoDTO? BrokerInfo { get; set; }
        public RiskSpecificRadiusOfOperationDto? RadiusOfOperations { get; set; }
        public RiskSpecificDriverHiringCriteriaDto? DriverHiringCriteria { get; set; }
        public RiskSpecificDriverInfoDto? DriverInfo { get; set; }
        public RiskSpecificDotInfoDto? DotInfo { get; set; }
        public RiskSpecificMaintenanceSafetyDTO MaintenanceSafety { get; set; }
        public RiskSpecificGeneralLiabilityCargoDTO GeneralLiabilityCargo { get; set; }
        public RiskSpecificUnderwritingQuestionDto? UnderwritingQuestions { get; set; }
        public FilingsInformationDTO FilingsInformation { get; set; }
        public NameAndAddressDTO NameAndAddress { get; set; }
        public BusinessDetailsDTO BusinessDetails { get; set; }

        public List<VehicleDTO> Vehicles { get; set; }
        public List<DriverDTO> Drivers { get; set; }
        public DriverHeaderDTO DriverHeader { get; set; }
        public List<RiskCoverageDTO> RiskCoverages { get; set; }
        public RiskCoverageDTO RiskCoverage { get; set; }
        public RiskCoverageDTO BoundRiskCoverage => RiskCoverages?.FirstOrDefault(x => x.OptionNumber == Binding?.BindOptionId);
        public RiskHistoryListDTO RiskHistory { get; set; }
        public List<ClaimsHistoryDTO> ClaimsHistory { get; set; }
        public List<RiskNotesDTO> RiskNotes { get; set; }
        public List<RiskFormDTO> RiskForms { get; set; }

        public BindingDTO Binding { get; set; }
        public List<BindingRequirementsDTO> BindingRequirements { get; set; }
        public List<QuoteConditionsDTO> QuoteConditions { get; set; }

        public BlanketCoverageDTO BlanketCoverage { get; set; }
        public List<RiskAdditionalInterestDTO> AdditionalInterest { get; set; }
        public List<RiskManuscriptsDTO> RiskManuscripts { get; set; }
        public List<FileUploadDocumentDTO> RiskDocuments { get; set; }

        public List<PremiumRaterVehicleFactorsDTO> VehicleFactors { get; set; }
        public DateTime? EndorsementEffectiveDate { get; set; }
        public DateTime? PrevEndorsementEffectiveDate { get; set; }

        public List<DriverDTO> RiskDetailDrivers { get; set; }
        public List<RiskSpecificDestinationDto> RiskSpecificDestinations { get; set; }
        public List<FieldVersionDTO> FieldVersions { get; set; }
        public int RaterVersionId { get; set; }
        public bool IsRaterVersionLocked { get; set; }
        public bool UpdateXPRaterVersion { get; set; }
        public bool IsRenewed { get; set; }
        public bool IsRenewal { get; set; }
        public bool IsFromRenewal { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string PreviousPolicyNumber { get; set; }
        public List<RiskRelatedDTO> RiskRelated { get; set; }
        public int? CountPowerUnit { get; set; }
        public int? CountTrailer { get; set; }
        public int? CountOthers { get; set; }
        public int? AssistantUnderwriterId { get; set; }
        public int? PsrUserId { get; set; }
        public int? TeamLeadUserId { get; set; }
        public int? InternalUWUserId { get; set; }
        public int? ProductUWUserId { get; set; }
        public DateTime? RequestedQuoteDate { get; set; }
    }
}
