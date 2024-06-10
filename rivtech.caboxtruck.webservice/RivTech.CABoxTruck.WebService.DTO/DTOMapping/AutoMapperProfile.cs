using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using RivTech.CABoxTruck.WebService.DTO.Notes;
using RivTech.CABoxTruck.WebService.DTO.Questions;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using System.Collections.Generic;
using RivTech.CABoxTruck.WebService.Data.Entity.Drivers;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.Data.DALDapperModels;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using RivTech.CABoxTruck.WebService.Common.ReportTemplates;

namespace RivTech.CABoxTruck.WebService.DTO.DTOMapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<AuditLog, AuditLogDTO>().ReverseMap();
            CreateMap<Entity, EntityDTO>().ReverseMap();
            CreateMap<EntityAddress, EntityAddressDTO>().ReverseMap();
            CreateMap<ErrorLog, ErrorLogDTO>().ReverseMap();
            CreateMap<Insured, InsuredDTO>().ReverseMap();
            CreateMap<RiskDetail, RiskDTO>().ReverseMap();
            CreateMap<LvAdditionalTerm, AdditionalTermDTO>().ReverseMap();
            CreateMap<BusinessType, BusinessTypeDTO>().ReverseMap();
            CreateMap<LvPaymentPlan, PaymentPlanDTO>().ReverseMap();
            CreateMap<LvRateType, RateTypeDTO>().ReverseMap();
            CreateMap<LvRelatedEntityType, RelatedEntityTypeDTO>().ReverseMap();
            CreateMap<LvRetroDateType, RetroDateTypeDTO>().ReverseMap();
            CreateMap<LvRiskStatusType, RiskStatusTypeDTO>().ReverseMap();
            CreateMap<LvRiskType, RiskTypeDTO>().ReverseMap();

            CreateMap<EntitySubsidiary, EntitySubsidiaryDTO>().ReverseMap();
            CreateMap<EntitySubsidiary, SaveEntitySubsidiaryDTO>().ReverseMap();
            CreateMap<EntityContact, EntityContactDTO>().ReverseMap();
            CreateMap<RiskAdditionalInterest, RiskAdditionalInterestDTO>().ReverseMap();

            CreateMap<Question, QuestionDTO>().ReverseMap();
            CreateMap<Validation, ValidationDTO>().ReverseMap();
            CreateMap<QuestionValidation, QuestionValidationDTO>().ReverseMap();
            CreateMap<RiskResponse, RiskResponseDTO>().ReverseMap();
            CreateMap<LvQuestionSection, EnumerationDTO>().ReverseMap();
            CreateMap<LvQuestionCategory, EnumerationDTO>().ReverseMap();
            CreateMap<LvQuestionType, EnumerationDTO>().ReverseMap();
            CreateMap<LvValidationType, EnumerationDTO>().ReverseMap();
            CreateMap<LvRiskResponseType, EnumerationDTO>().ReverseMap();

            CreateMap<RiskCoverage, RiskCoverageDTO>().ReverseMap();
            CreateMap<RiskCoverage, SaveRiskCoverageDTO>().ReverseMap();
            CreateMap<RiskHistory, RiskHistoryDTO>().ReverseMap();
            CreateMap<ClaimsHistory, ClaimsHistoryDTO>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<DriverIncidents, DriverIncidentsDTO>().ReverseMap();

            CreateMap<Entity, DriverDTO>().ReverseMap();
            //CreateMap<Driver, DriverDTO>().ReverseMap();

            CreateMap<Driver, DriverDTO>()
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.Entity.FirstName))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.Entity.LastName))
                .ForMember(dest => dest.MiddleName, opts => opts.MapFrom(src => src.Entity.MiddleName))
                .ForMember(dest => dest.BirthDate, opts => opts.MapFrom(src => src.Entity.BirthDate))
                .ForMember(dest => dest.Age, opts => opts.MapFrom(src => src.Entity.Age))
                .ReverseMap();

            CreateMap<DriverHeader, DriverHeaderDTO>().ReverseMap();
            var vehicleMap = CreateMap<Vehicle, VehicleDTO>(); 
            vehicleMap.ReverseMap();
            // one-way map vehicle premium to dto.
            vehicleMap
                .ForMember(dest => dest.AutoLiabilityPremium, opts => opts.MapFrom(src => src.RiskVehiclePremiumTransaction.CurrentALPremium))
                .ForMember(dest => dest.PhysicalDamagePremium, opts => opts.MapFrom(src => src.RiskVehiclePremiumTransaction.CurrentPDPremium))
                .ForMember(dest => dest.CargoPremium, opts => opts.MapFrom(src => src.RiskVehiclePremiumTransaction.CurrentCargoPremium))
                .ForMember(dest => dest.RefrigerationPremium, opts => opts.MapFrom(src => src.RiskVehiclePremiumTransaction.CurrentRefPremium))
                .ForMember(dest => dest.ComprehensivePremium, opts => opts.MapFrom(src => src.RiskVehiclePremiumTransaction.CurrentCompPremium))
                .ForMember(dest => dest.CollisionPremium, opts => opts.MapFrom(src => src.RiskVehiclePremiumTransaction.CurrentCollPremium))
                .ForMember(dest => dest.TotalPremium, opts => opts.MapFrom(src => src.RiskVehiclePremiumTransaction.CurrentPremium));

            CreateMap<RiskSpecificDestination, RiskSpecificDestinationDto>().ReverseMap();
            CreateMap<RiskSpecificCommoditiesHauled, RiskSpecificCommoditiesHauledDto>().ReverseMap();
            CreateMap<RiskSpecificDriverInfo, RiskSpecificDriverInfoDto>().ReverseMap();
            CreateMap<RiskSpecificRadiusOfOperation, RiskSpecificRadiusOfOperationDto>().ReverseMap();
            CreateMap<RiskSpecificSafetyDevice, RiskSpecificSafetyDeviceDTO>().ReverseMap();
            CreateMap<LvRiskSpecificSafetyDeviceCategory, EnumerationDTO>().ReverseMap();
            CreateMap<RiskSpecificDriverHiringCriteria, RiskSpecificDriverHiringCriteriaDto>().ReverseMap();
            CreateMap<BackgroundCheckOption, EnumerationDTO>().ReverseMap();
            CreateMap<DriverTrainingOption, EnumerationDTO>().ReverseMap();
            CreateMap<MvrPullingFrequencyOption, EnumerationDTO>().ReverseMap();
            CreateMap<RiskSpecificUnderwritingQuestion, RiskSpecificUnderwritingQuestionDto>().ReverseMap();

            CreateMap<BrokerInfo, BrokerInfoDTO>().ReverseMap();
            CreateMap<RiskDetail, FilingsInformationDTO>().ForMember(dest => dest.RiskDetailId, opts => opts.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<RiskDetail, BlanketCoverageDTO>().ForMember(dest => dest.RiskDetailId, opts => opts.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<LvReasonMove, ReasonMoveDTO>().ReverseMap();

            CreateMap<Agency, AgencyDTO>().ReverseMap();
            CreateMap<SubAgency, SubAgencyDTO>().ReverseMap();
            CreateMap<Agent, AgentDTO>().ReverseMap();
            CreateMap<SubAgent, AgentSubAgencyDTO>().ReverseMap();

            CreateMap<RiskFiling, RiskFilingDTO>().ReverseMap();

            CreateMap<RiskSpecificDotInfo, RiskSpecificDotInfoDto>().ReverseMap();
            CreateMap<ChameleonIssuesOption, EnumerationDTO>().ReverseMap();
            CreateMap<SaferRatingOption, EnumerationDTO>().ReverseMap();
            CreateMap<TrafficLightRatingOption, EnumerationDTO>().ReverseMap();

            CreateMap<VehiclePremiumRatingFactor, PremiumRaterVehicleFactorsDTO>().ReverseMap();
            CreateMap<DriverPremiumRatingFactor, PremiumRaterDriverFactorsDTO>().ReverseMap();

            CreateMap<LvDataset, LvDatasetDTO>().ReverseMap();
            CreateMap<LvLimits, LimitsDTO>().ReverseMap();
            CreateMap<RiskNotes, RiskNotesDTO>().ReverseMap();
            CreateMap<NoteCategoryOption, NoteCategoryOptionDTO>().ReverseMap();

            CreateMap<RiskForm, RiskFormDTO>().ReverseMap();
            CreateMap<Form, FormDTO>().ReverseMap();
            CreateMap<FormVersion, FormVersionDTO>().ReverseMap();

            CreateMap<Binding, BindingDTO>().ReverseMap();
            CreateMap<BindingRequirements, BindingRequirementsDTO>().ReverseMap();
            CreateMap<FileUploadDocument, FileUploadDocumentDTO>().ReverseMap();
            CreateMap<List<FileUploadDocumentDTO>, List<FileUploadDocumentDTO>>().ReverseMap();
            CreateMap<QuoteConditions, QuoteConditionsDTO>().ReverseMap();
            CreateMap<QuoteConditionsOption, EnumerationDTO>().ReverseMap();

            CreateMap<PolicyContacts, PolicyContactsDTO>().ReverseMap();

            //do not remove. mapping from parent.property to parent
            CreateMap<FileUploadDocumentDTO, FileUploadDocumentDTO>().ReverseMap();
            CreateMap<List<FileUploadDocumentDTO>, BindingRequirementsDTO>()
            .ForMember(dest => dest.FileUploads, opts => opts.MapFrom(src => src))
            .ReverseMap();

            CreateMap<List<FileUploadDocumentDTO>, QuoteConditionsDTO>()
            .ForMember(dest => dest.FileUploads, opts => opts.MapFrom(src => src))
            .ReverseMap();

            CreateMap<FeeKindData, FeeKind>().ReverseMap();
            CreateMap<TaxDetailsBreakdownData, TaxDetailsBreakdown>().ReverseMap();
            CreateMap<TaxDetailsData, TaxDetails>()
                //.ForMember(x => x.TaxDetailsBreakdown, y => y.MapFrom(m => m.TaxDetailsBreakdown))
                .ReverseMap();
            CreateMap<TaxDetailsData, TaxDetailsDTO>()
                //.ForMember(x => x.TaxDetailsBreakdowns, y => y.MapFrom(m => m.TaxDetailsBreakdowns))
                .ReverseMap();
            CreateMap<TaxDetailsBreakdownData, TaxDetailsBreakdownDTO>()
                .ReverseMap();

            CreateMap<SLNumber, SLNumberDTO>().ReverseMap();
            CreateMap<Banner, BannerDTO>().ReverseMap();

            CreateMap<RiskFormOtherDTO, ExclusionRadiusDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, HiredPhysdamDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, DriverRequirementDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, TerritoryExclusionDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, TrailerInterchangeDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, GeneralLiabilityDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, Mcs90DTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormUIIADTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormDriverExclusionDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormLossPayeeDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormBaseDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormUMBIDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormPIPDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormTXUMUIMDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormNJUMUIMDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormUMDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormUIMDTO>().ReverseMap(); 
            CreateMap<RiskFormOtherDTO, FormPAaddcombFPBDTO>().ReverseMap(); 
            CreateMap<RiskFormOtherDTO, FormUMUIMBIDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormUMBIPDDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormMPDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, FormUIMBIPDDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, CoveredAutoDesigSymbolDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, TrailerInterchangeALDTO>().ReverseMap();
            CreateMap<RiskFormOtherDTO, PollutionCoverageDTO>().ReverseMap();
            CreateMap<DriverDTO, FormDriverDTO>().ReverseMap();
            CreateMap<RiskDetailDriver, DriverDTO>().IncludeMembers(s => s.Driver);

            CreateMap<RiskManuscriptsDTO, RiskManuscripts>().ReverseMap();

            CreateMap<PolicyHistoryDTO, PolicyHistory>().ReverseMap();
            CreateMap<BillingBindResponseDTO, InvoiceDTO>().ReverseMap();
            CreateMap<EndorsementPendingChange, EndorsementPendingChangeDTO>().ReverseMap();

            CreateMap<Retailer, RetailerDTO>().ReverseMap();
            CreateMap<RetailerAgency, RetailerAgencyDTO>().ReverseMap();
            CreateMap<RetailerAgent, RetailerAgentDTO>().ReverseMap();
            CreateMap<BlacklistedDriverDTO, BlacklistedDriver>().ReverseMap();
            CreateMap<RiskDetail, PaymentPortalSearchResponseDTO>().ReverseMap();
            CreateMap<EmailQueue, EmailQueueDTO>().ReverseMap();
            CreateMap<EmailQueueArchive, EmailQueueArchiveDTO>().ReverseMap();
            CreateMap<FTPDocument, FTPDocumentDTO>().ReverseMap();
            CreateMap<FTPDocumentTemporary, FTPDocumentDTO>().ReverseMap();
            CreateMap<FTPDocumentFilesCount, FTPDocumentFilesCountDTO>().ReverseMap();
            CreateMap<FTPDocumentFilesCountDTO, CovenirReportDTO>().ReverseMap();

            //DALDapper
            CreateMap<DMVReportModel, DMVReportDTO>().ReverseMap();
            CreateMap<POMReportModel, POMReportDTO>().ReverseMap();

            CreateMap<SymbolTenResponse, SymbolTenResponseDTO>().ReverseMap();
            CreateMap<FieldVersion, FieldVersionDTO>().ReverseMap();
        }
    }
}
