using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NPOI.SS.Formula.Functions;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class FormsService : IFormsService
    {
        private readonly IFormRepository _formRepository;
        private readonly IMapper _mapper;
        private readonly INameAndAddressService _namedAddressService;
        private readonly IRiskFormRepository _riskFormRepository;
        private readonly IQuoteOptionsService _quoteOptionsService;
        private readonly IBindingService _bindingService;
        private readonly IBrokerInfoService _brokerInfoService;
        private readonly ILog4NetService _errorLogservice;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IEndorsementChangesService _endorsementChangesService;
        private readonly IEndorsementChangeDeserializer _endorsementChangeDeserializer;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;
        private readonly AppDbContext _context;

        public FormsService(AppDbContext context,
                            IRiskFormRepository riskFormRepository, 
                            IFormRepository formRepository, 
                            INameAndAddressService namedAddressService, 
                            IQuoteOptionsService quoteOptionsService,
                            IBindingService bindingService,
                            IBrokerInfoService brokerInfoService,
                            IRiskDetailRepository riskDetailRepository,
                            IEndorsementChangesService endorsementChangesService,
                            IEndorsementChangeDeserializer endorsementChangeDeserializer,
                            IMapper mapper, ILog4NetService errorLogservice,
                            IRiskRepository riskRepository,
                            IRiskManuscriptsRepository riskManuscriptsRepository
        ) {
            _riskFormRepository = riskFormRepository;
            _formRepository = formRepository;
            _namedAddressService = namedAddressService;
            _quoteOptionsService = quoteOptionsService;
            _bindingService = bindingService;
            _brokerInfoService = brokerInfoService;
            _errorLogservice = errorLogservice;
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _endorsementChangesService = endorsementChangesService;
            _endorsementChangeDeserializer = endorsementChangeDeserializer;
            _riskManuscriptsRepository = riskManuscriptsRepository;
            _mapper = mapper;
            _context = context;
        }

        public JsonSerializerSettings JsonSerializerSettings =>
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new DecimalFormatConverter() }
            };

        public async Task<List<RiskFormDTO>> GetByRiskIdAsync(Guid riskId)
        {
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskId && x.Form.IsActive, r => r.Form.FormVersions);
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskId);
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);

            foreach (var riskForm in riskForms)
            {
                var formVersion = riskForm.Form.FormVersions?.FirstOrDefault(fv => riskDetail.RequestedQuoteDate > fv.EffectiveDate);

                if(riskDetail.RequestedQuoteDate == null && !risk.IsRaterVersionLocked)
                    formVersion = riskForm.Form.FormVersions?.OrderByDescending(fv => fv.Version)?.FirstOrDefault();

                if (formVersion != null)
                    riskForm.Form.FormNumber = formVersion.FormNumber;
            }

            var riskFormsResult = _mapper.Map<List<RiskFormDTO>>(riskForms);
            await MergeManuscript(riskFormsResult, riskId, riskDetail.RequestedQuoteDate, risk.IsRaterVersionLocked);

            return riskFormsResult;
        }

        public async Task<List<RiskFormDTO>> GetSelectedByRiskIdAsync(Guid riskId)
        {
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskId && x.Form.IsActive && x.IsSelected == true, r => r.Form.FormVersions);
            return _mapper.Map<List<RiskFormDTO>>(riskForms);
        }

        public async Task<bool> HasInitialDriverExclusionFormAsync(Guid riskId)
        {
            var riskDetail = await _riskDetailRepository.GetFirstRiskDetailByRiskIdAsync(riskId);
            var riskForm = await _riskFormRepository.FindAsync(x => x.RiskDetailId == riskDetail.Id && x.Form.IsActive && x.IsSelected == true && x.FormId == "DriverExclusion");
            if (riskForm != null) return riskForm.Other != null;
            return false;
        }

        public async Task<List<RiskFormDTO>> UpdateAsync(List<RiskFormDTO> riskForms)
        {
            foreach (var riskForm in riskForms)
            {
                var entity =
                    await _riskFormRepository.FindAsync(e =>
                        e.RiskDetailId == riskForm.RiskDetailId && e.FormId == riskForm.FormId);
                if (entity == null) continue;
                entity.IsSelected = riskForm.IsSelected;
                entity.Other = riskForm.Other;
                _riskFormRepository.Update(entity);
            }

            await _riskFormRepository.SaveChangesAsync();
            return riskForms;
        }

        public async Task<bool> UpdateSelectedAsync(List<RiskFormDTO> riskForms)
        {
            foreach (var riskForm in riskForms)
            {
                //Exclude from auto select
                switch (riskForm.FormId)
                {
                    case "CoveredAutoDesignationSymbol":
                        continue;
                    default:
                        break;
                }

                var entity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == riskForm.RiskDetailId && e.FormId == riskForm.FormId);
                if (entity == null) continue;
                entity.IsSelected = riskForm.IsSelected;
                _riskFormRepository.Update(entity);
            }
            await _riskFormRepository.SaveChangesAsync();
            return true;
        }

        public async Task<RiskFormDTO> InsertUserFormAsync(RiskFormDTO riskForm)
        {
            var maxSortOrder = await _formRepository.GetAsync(f => f.IsActive);
            //var newId = Guid.NewGuid().ToString();
            var entity = _mapper.Map<RiskForm>(riskForm);
            entity.IsSelected = true;
            entity.Form.Id = entity.FormId;
            entity.Form.SortOrder = maxSortOrder.Max(s => s.SortOrder) + 1;
            entity.Form.IsActive = true;
            entity.Form.DecPageOrder = 1;

            var newRiskForm = await _riskFormRepository.AddAsync(entity);
            await _riskFormRepository.SaveChangesAsync();

            return _mapper.Map<RiskFormDTO>(newRiskForm);
        }

        public async Task<List<RiskFormDTO>> InsertDefaultsAsync(Guid riskId)
        {
            var forms = await _formRepository.GetAsync(f => f.IsActive && !f.IsSupplementalDocument);
            var userStates = await GetUserStates(riskId);
            var baseForms = new List<RiskForm>();
            var symbolTenResponses = await GetSymbolTenResponses(riskId);
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskId);
            var riskCoverage = riskDetail.RiskCoverages[0];
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);

            foreach (var form in forms)
            {
                var riskForm = new RiskForm
                {
                    Id = Guid.NewGuid(),
                    RiskDetailId = riskId,
                    FormId = form.Id,
                    CreatedBy = 0,
                    CreatedDate = DateTime.Now,
                    Form = form,
                    IsSelected = form.IsDefault
                };
                riskForm.Other = form.Id switch
                {
                    "GeneralLiabilityEndorsement" => JsonConvert.SerializeObject(
                        new GeneralLiabilityDTO { InjuryLimit = 1000000, MedicalExpenses = 5000, DamagesRentedToYou = 100000, AggregateLimit = 2000000 }, JsonSerializerSettings),
                    "ExclusionRadiusEndorsement" => JsonConvert.SerializeObject(
                        new ExclusionRadiusDTO {Radius = 300}, JsonSerializerSettings),
                    "TerritoryExclusion" => JsonConvert.SerializeObject(
                        new TerritoryExclusionDTO { States = string.Join(",", userStates.Distinct()), StatesDefault = true}, JsonSerializerSettings),
                    "DriverRequirements" => JsonConvert.SerializeObject(
                        new DriverRequirementDTO
                        {
                            Requirement = "No more than one (1) moving violation or at-fault accident in the last thirty-six (36) months"
                        }, JsonSerializerSettings),
                    "CoveredAutoDesignationSymbol" => JsonConvert.SerializeObject(
                        new CoveredAutoDesigSymbolDTO { CoveredAutoText = symbolTenResponses?.FirstOrDefault()?.Id.ToString(), TextForEndorsement = null }, JsonSerializerSettings),
                    _ => riskForm.Other
                };

                if (form.Id == "CoveredAutoDesignationSymbol" && (riskCoverage?.ALSymbolId == 14 || riskCoverage?.ALSymbolId == 15))
                    riskForm.IsSelected = true;

                baseForms.Add(riskForm);
            }

            baseForms.ForEach(rf => rf.Form = null);
            await _riskFormRepository.AddRangeAsync(baseForms);
            await _riskFormRepository.SaveChangesAsync();
            var result = await _riskFormRepository.GetByIdIncludeAsync(riskId);

            //Update displayed form number depending on version
            foreach (var riskForm in result)
            {
                var formVersion = riskForm.Form.FormVersions?.FirstOrDefault(fv => riskDetail.RequestedQuoteDate > fv.EffectiveDate);

                if (riskDetail.RequestedQuoteDate == null && !risk.IsRaterVersionLocked)
                    formVersion = riskForm.Form.FormVersions?.OrderByDescending(fv => fv.Version)?.FirstOrDefault();

                if (formVersion != null)
                    riskForm.Form.FormNumber = formVersion.FormNumber;
            }

            var riskFormsResult = _mapper.Map<List<RiskFormDTO>>(result);
            await MergeManuscript(riskFormsResult, riskId, riskDetail.RequestedQuoteDate, risk.IsRaterVersionLocked);
            return riskFormsResult;
        }

        public async Task<RiskFormDTO> UpdateDefaultStatesAsync(Guid id)
        {
            var entity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == id && e.FormId == "TerritoryExclusion");
            if (entity == null) return null;

            var userStates = await GetUserStates(id);
            entity.Other = JsonConvert.SerializeObject(
                new TerritoryExclusionDTO {States = string.Join(",", userStates.Distinct()), StatesDefault = true},
                JsonSerializerSettings);

            await _riskFormRepository.SaveChangesAsync();
            return _mapper.Map<RiskFormDTO>(entity);
        }

        public async Task<RiskFormDTO> SingleFormUpdateAsync(RiskFormDTO riskForm)
        {
            var entity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == riskForm.RiskDetailId && e.FormId == riskForm.FormId);

            if(entity != null)
            {
                if (entity.IsSelected == riskForm.IsSelected) return riskForm;
                entity.IsSelected = riskForm.IsSelected;

                if (riskForm.IsSelected == false)
                {
                    switch (entity.FormId)
                    {
                        case "HiredPhysicalDamage":
                            await _quoteOptionsService.RemoveHiredPhysicalDamage(riskForm.RiskDetailId);
                            break;
                        case "TrailerInterchangeCoverage":
                            await _quoteOptionsService.RemoveTrailerInterchange(riskForm.RiskDetailId);
                            break;
                        case "CoveredAutoDesignationSymbol":
                            entity.Other = null;
                            break;
                        default:
                            break;
                    }
                }

                _riskFormRepository.Update(entity);
                await _riskFormRepository.SaveChangesAsync();
            }
            return riskForm;
        }

        public async Task<RiskFormDTO> UpdateOtherAsync<T>(RiskFormOtherDTO resource)
        {
            resource.UpdatedDate = null;
            var otherJson = JsonConvert.SerializeObject(_mapper.Map<T>(resource), JsonSerializerSettings);

            var entity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == resource.FormId);
            entity.Other = otherJson;
            entity.IsSelected = true;

            _riskFormRepository.Update(entity);
            await _riskFormRepository.SaveChangesAsync();
            return _mapper.Map<RiskFormDTO>(entity);
        }

        public async Task SaveFormsContentAsync(RiskDetail riskDetail, Guid? currentRiskDetailId = null, 
            DateTime? effectiveDate = null, List<DriverDTO> initialExcludedDrivers = null, bool isEffectiveDateChanged = false)
        {
            try
            {
                var risk = _mapper.Map<RiskDTO>(riskDetail);
                var policy = await _riskRepository.FindAsync(risk.RiskId);
                var hasRiskForm = await _riskFormRepository.AnyAsync(e => e.RiskDetailId == riskDetail.Id);

                if (isEffectiveDateChanged && !hasRiskForm) return;

                risk.Binding = await _bindingService.GetBindingByRiskIdAsync(riskDetail.RiskId);
                risk.BrokerInfo = await _brokerInfoService.GetByRiskDetailIdAsync(riskDetail.RiskId);

                var optionId = (risk.Binding?.BindOptionId ?? 1).ToString();
                var optionIndex = int.Parse(optionId) - 1;

                var isEndorsement = effectiveDate != null;
                effectiveDate = (isEndorsement ? effectiveDate : risk.BrokerInfo?.EffectiveDate) ?? DateTime.Now;

                if (risk.RiskCoverages == null || !risk.RiskCoverages.Any()) throw new Exception("Missing RiskCoverages");

                var coverage = optionIndex < risk.RiskCoverages.Count ? risk.RiskCoverages[optionIndex] : risk.RiskCoverages[0];

                //var excludeDrivers = isEndorsement
                //    ? risk.RiskDetailDrivers.Where(d => 
                //        IsDriverDeletedOrInKoRule(d, effectiveDate.Value, risk.RiskDetailDrivers.Select(rd => rd.PreviousDriverVersionId.GetValueOrDefault()).ToList()) 
                //        || (!policy.IsIssued() && (d.IsActive != false && (d.Options == null || !d.Options.Contains(optionId))))
                //    ) : initialExcludedDrivers.ToList();
                
                var resource = new RiskFormOtherDTO
                {
                    RiskDetailId = risk.Id.GetValueOrDefault(),
                    AutoBILimitId = coverage?.AutoBILimitId,
                    UMBILimitId = coverage?.UMBILimitId,
                    UMPDLimitId = coverage?.UMPDLimitId,
                    GLBILimitId = coverage?.GLBILimitId,
                    PIPLimitId = coverage?.PIPLimitId,
                    UIMBILimitId = coverage?.UIMBILimitId,
                    UIMPDLimitId = coverage?.UIMPDLimitId,
                    MPLimitId = coverage?.MedPayLimitId,
                    TrailerInterchangeLimitId = coverage.TrailerInterchangeLimitId,
                    PollutionCovLimitId = coverage.PollutionCovLimitId,
                    //Drivers = _mapper.Map<List<FormDriverDTO>>(excludeDrivers.Distinct()),
                    UpdatedDate = effectiveDate
                };

                var hasPhysicalDamage = coverage != null && (coverage.CompDeductibleId > LimitConstants.ComprehensiveNoCoverageId
                                                             || coverage.FireDeductibleId > LimitConstants.ComprehensiveNoCoverageId
                                                             || coverage.CollDeductibleId > LimitConstants.CollisionNoCoverageId);

                var endorsementChanges = await _endorsementChangesService.GetEndorsementChangesByRiskId(riskDetail.RiskId, riskDetail.EndorsementNumber);
                var changeCollection = _endorsementChangeDeserializer.Deserialize(endorsementChanges);

                bool hasOccupantChanges, hasPhysicalDamageLimitOfInsurance, hasDriverExclusionUnder23, hasDriverExclusion,
                    hasCourierCoverageEndorsement, hasRefrigerationCargoEndorsement,
                    hasHired, hasGL, hasGL2, hasDriverReq, hasExclusion, hasTrailer, hasTerritory, hasMotorCargo,
                    hasPermissive, hasUnreportedVehDed, hasUnreportedDrvDed, hasFormFMTCBIPD, hasMotorCarrierEndorsement;
                hasOccupantChanges = hasPhysicalDamageLimitOfInsurance = hasDriverExclusion =
                    hasDriverExclusionUnder23 = hasCourierCoverageEndorsement = hasRefrigerationCargoEndorsement = hasHired = 
                    hasGL = hasGL2 = hasDriverReq = hasExclusion = hasTrailer = hasTerritory = hasMotorCargo =
                    hasPermissive = hasUnreportedVehDed = hasUnreportedDrvDed = hasFormFMTCBIPD = hasMotorCarrierEndorsement =
                    isEffectiveDateChanged ? true : false;

                bool hasPAAddCombfirstpartybenefits = isEffectiveDateChanged ? true : false;
                bool hasCoveredAutoDesignationSymbol = isEffectiveDateChanged ? true : false;

                if (changeCollection != null)
                {
                    foreach (IEndorsementChangeItem changeItem in changeCollection)
                    {
                        if (changeItem is FormSelectionChangeItem)
                        {
                            FormSelectionChangeItem change = (FormSelectionChangeItem)changeItem;
                            if (change.Id == "PhysicalDamageLimitOfInsurance") hasPhysicalDamageLimitOfInsurance = true;
                            if (change.Id == "DriverExclusionUnder23") hasDriverExclusionUnder23 = true;
                            if (change.Id == "CourierCoverageEndorsement") hasCourierCoverageEndorsement = true;
                            if (change.Id == "OccupantHazardExclusionEndorsement") hasOccupantChanges = true;
                            if (change.Id == "RefrigerationCargoEndorsement") hasRefrigerationCargoEndorsement = true;
                            if (change.Id == "MotorTruckCargoEndorsement") hasMotorCargo = true;
                            if (change.Id == "PermissiveExclusionEndorsement") hasPermissive = true;
                            if (change.Id == "UnreportedVehicleDeductible") hasUnreportedVehDed = true;
                            if (change.Id == "UnreportedDriverDeductible") hasUnreportedDrvDed = true;
                            if (change.Id == "FormFMotorCarrierBIPD") hasFormFMTCBIPD = true;
                            if (change.Id == "MotorCarrierEndorsement") hasMotorCarrierEndorsement = true;
                        }

                        if (changeItem is FormHiredPhysicalDamageEndorsementChangeItem)
                            hasHired = true;
                        if (changeItem is FormGeneralLiabilityEndorsementChangeItem)
                            hasGL = true;
                        if (changeItem is FormDriverRequirementsEndorsementChangeItem)
                            hasDriverReq = true;
                        if (changeItem is FormExclusionRadiusEndorsementChangeItem)
                            hasExclusion = true;
                        if (changeItem is FormTrailerInterchangeCoverageChangeItem)
                            hasTrailer = true;
                        if (changeItem is FormTerritoryExclusionEndorsementChangeItem)
                            hasTerritory = true;
                        if (changeItem is CoverageLimitsChangeItem)
                        {
                            CoverageLimitsChangeItem change = (CoverageLimitsChangeItem)changeItem;
                            if (change.GLBILimitId != null)
                                hasGL2 = true;
                        }
                        if (changeItem is FormPennsylvaniaAddCombFPBChangeItem)
                            hasPAAddCombfirstpartybenefits = true;

                        if (changeItem is CoveredAutoDesignationSymbolChangeItem)
                            hasCoveredAutoDesignationSymbol = true;
                    }
                }
                var pdLimitEntity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "PhysicalDamageLimitOfInsurance");
                if (pdLimitEntity != null) pdLimitEntity.IsSelected = hasPhysicalDamage;
                SetUpdatedDate<FormBaseDTO>(pdLimitEntity, resource, hasPhysicalDamageLimitOfInsurance);

                var driverUnder23 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "DriverExclusionUnder23");
                SetUpdatedDate<FormBaseDTO>(driverUnder23, resource, hasDriverExclusionUnder23);

                var courierCoverage = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "CourierCoverageEndorsement");
                SetUpdatedDate<FormBaseDTO>(courierCoverage, resource, hasCourierCoverageEndorsement);

                var occupantHazard = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "OccupantHazardExclusionEndorsement");
                SetUpdatedDate<FormBaseDTO>(occupantHazard, resource, hasOccupantChanges);

                var refCargo = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "RefrigerationCargoEndorsement");
                SetUpdatedDate<FormBaseDTO>(refCargo, resource, hasRefrigerationCargoEndorsement);

                var motorTruckCargo = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "MotorTruckCargoEndorsement");
                SetUpdatedDate<FormBaseDTO>(motorTruckCargo, resource, hasMotorCargo);

                var hiredEntity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "HiredPhysicalDamage");
                SetUpdatedDate<HiredPhysdamDTO>(hiredEntity, resource, hasHired);

                var generalLiab = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "GeneralLiabilityEndorsement");
                SetUpdatedDate<GeneralLiabilityDTO>(generalLiab, resource, hasGL);

                var driverReqEntity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "DriverRequirements");
                SetUpdatedDate<DriverRequirementDTO>(driverReqEntity, resource, hasDriverReq);

                var exclusionRadius = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "ExclusionRadiusEndorsement");
                SetUpdatedDate<ExclusionRadiusDTO>(exclusionRadius, resource, hasExclusion);

                var trailerEntity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "TrailerInterchangeCoverage");
                SetUpdatedDate<TrailerInterchangeDTO>(trailerEntity, resource, hasTrailer);

                var territoryEntity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "TerritoryExclusion");
                SetUpdatedDate<TerritoryExclusionDTO>(territoryEntity, resource, hasTerritory);


                var driverEntity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "DriverExclusion");
                SetUpdatedDate<FormDriverExclusionDTO>(driverEntity, resource, driverEntity.IsSelected ?? false);

                var formPAaddcombFPB = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "PennsylvaniaAddCombFirstPartyBenefitsEndorsement");
                SetUpdatedDate<FormPAaddcombFPBDTO>(formPAaddcombFPB, resource, hasPAAddCombfirstpartybenefits);

                var coveredAutoDesignationSymbol = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "CoveredAutoDesignationSymbol");
                SetUpdatedDate<CoveredAutoDesigSymbolDTO>(coveredAutoDesignationSymbol, resource, hasCoveredAutoDesignationSymbol);

                var permissive = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "PermissiveExclusionEndorsement");
                SetUpdatedDate<FormBaseDTO>(permissive, resource, hasPermissive);

                var underportedVehDed = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "UnreportedVehicleDeductible");
                SetUpdatedDate<FormBaseDTO>(underportedVehDed, resource, hasUnreportedVehDed);

                var unreportedDriverDed = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "UnreportedDriverDeductible");
                SetUpdatedDate<FormBaseDTO>(unreportedDriverDed, resource, hasUnreportedDrvDed);

                var formFMTCBIPD = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "FormFMotorCarrierBIPD");
                SetUpdatedDate<FormBaseDTO>(formFMTCBIPD, resource, hasFormFMTCBIPD);

                var motorCarrierEndorsement = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "MotorCarrierEndorsement");
                SetUpdatedDate<FormBaseDTO>(motorCarrierEndorsement, resource, hasMotorCarrierEndorsement);

                if (currentRiskDetailId.HasValue)
                {
                    var pdLimitEntityC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "PhysicalDamageLimitOfInsurance");
                    if (pdLimitEntityC != null) pdLimitEntityC.IsSelected = hasPhysicalDamage;
                    SetUpdatedDate<FormBaseDTO>(pdLimitEntityC, resource, hasPhysicalDamageLimitOfInsurance);

                    var driverUnder23C = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "DriverExclusionUnder23");
                    SetUpdatedDate<FormBaseDTO>(driverUnder23C, resource, hasDriverExclusionUnder23);

                    var courierCoverageC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "CourierCoverageEndorsement");
                    SetUpdatedDate<FormBaseDTO>(courierCoverageC, resource, hasCourierCoverageEndorsement);

                    var occupantHazardC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "OccupantHazardExclusionEndorsement");
                    SetUpdatedDate<FormBaseDTO>(occupantHazardC, resource, hasOccupantChanges);

                    var refCargoC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "RefrigerationCargoEndorsement");
                    SetUpdatedDate<FormBaseDTO>(refCargoC, resource, hasRefrigerationCargoEndorsement);

                    var motorTruckCargoC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "MotorTruckCargoEndorsement");
                    SetUpdatedDate<FormBaseDTO>(motorTruckCargoC, resource, hasMotorCargo);

                    var hiredEntityC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "HiredPhysicalDamage");
                    SetUpdatedDate<HiredPhysdamDTO>(hiredEntityC, resource, hasHired);

                    var generalLiabC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "GeneralLiabilityEndorsement");
                    SetUpdatedDate<GeneralLiabilityDTO>(generalLiabC, resource, hasGL);

                    var driverReqEntityC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "DriverRequirements");
                    SetUpdatedDate<DriverRequirementDTO>(driverReqEntityC, resource, hasDriverReq);

                    var exclusionRadiusC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "ExclusionRadiusEndorsement");
                    SetUpdatedDate<ExclusionRadiusDTO>(exclusionRadiusC, resource, hasExclusion);

                    var trailerEntityC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "TrailerInterchangeCoverage");
                    SetUpdatedDate<TrailerInterchangeDTO>(trailerEntityC, resource, hasTrailer);

                    var territoryEntityC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "TerritoryExclusion");
                    SetUpdatedDate<TerritoryExclusionDTO>(territoryEntityC, resource, hasTerritory);

                    var formPAaddcombFPB2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "PennsylvaniaAddCombFirstPartyBenefitsEndorsement");
                    SetUpdatedDate<FormPAaddcombFPBDTO>(formPAaddcombFPB2, resource, hasPAAddCombfirstpartybenefits);

                    var coveredAutoDesignationSymbolC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "CoveredAutoDesignationSymbol");
                    SetUpdatedDate<CoveredAutoDesigSymbolDTO>(coveredAutoDesignationSymbolC, resource, hasCoveredAutoDesignationSymbol);

                    var permissiveC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "PermissiveExclusionEndorsement");
                    SetUpdatedDate<FormBaseDTO>(permissiveC, resource, hasPermissive);

                    var underportedVehDedC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "UnreportedVehicleDeductible");
                    SetUpdatedDate<FormBaseDTO>(underportedVehDedC, resource, hasUnreportedVehDed);

                    var unreportedDriverDedC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "UnreportedDriverDeductible");
                    SetUpdatedDate<FormBaseDTO>(unreportedDriverDedC, resource, hasUnreportedDrvDed);

                    var formFMTCBIPDC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "FormFMotorCarrierBIPD");
                    SetUpdatedDate<FormBaseDTO>(formFMTCBIPDC, resource, hasFormFMTCBIPD);

                    var motorCarrierEndorsementC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "MotorCarrierEndorsement");
                    SetUpdatedDate<FormBaseDTO>(motorCarrierEndorsementC, resource, hasMotorCarrierEndorsement);
                }

                var glJson = JsonConvert.SerializeObject(_mapper.Map<GeneralLiabilityDTO>(resource), JsonSerializerSettings);
                var glEntity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "GeneralLiabilityEndorsement");
                if (glEntity != null && hasGL2)
                {
                    if (glEntity.Other == null) glEntity.Other = glJson;
                    else
                    {
                        var entityOther = JsonConvert.DeserializeObject<GeneralLiabilityDTO>(glEntity.Other);
                        entityOther.UpdatedDate = resource.UpdatedDate;
                        glEntity.Other = JsonConvert.SerializeObject(_mapper.Map<GeneralLiabilityDTO>(entityOther), JsonSerializerSettings);
                    }
                }

                var uiiaJson = JsonConvert.SerializeObject(_mapper.Map<FormUIIADTO>(resource), JsonSerializerSettings);
                var uiiaEntity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "UIIAUpdated");
                if (uiiaEntity != null)
                {
                    if (uiiaEntity.IsSelected == false) uiiaEntity.Other = null;
                    else
                    {
                        if (uiiaEntity.Other == null) uiiaEntity.Other = uiiaJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIIADTO>(uiiaEntity.Other);
                            if (entityOther.AutoBILimitId != resource.AutoBILimitId) uiiaEntity.Other = uiiaJson;

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                uiiaEntity.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIIADTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var uninsuredMotoristBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(resource), JsonSerializerSettings);
                var cauninsuredMotoristBI = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "UninsuredMotoristCoverageBodilyInjury");
                if (cauninsuredMotoristBI != null)
                {
                    if (cauninsuredMotoristBI.IsSelected == false) cauninsuredMotoristBI.Other = null;
                    else
                    {
                        if (cauninsuredMotoristBI.Other == null) cauninsuredMotoristBI.Other = uninsuredMotoristBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(cauninsuredMotoristBI.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId) cauninsuredMotoristBI.Other = uninsuredMotoristBIJson;

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                cauninsuredMotoristBI.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var splituninsuredMotoristBI = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "SplitBodilyInjuryUninsuredMotoristCoverage");
                if (splituninsuredMotoristBI != null)
                {
                    if (splituninsuredMotoristBI.IsSelected == false) splituninsuredMotoristBI.Other = null;
                    else
                    {
                        if (splituninsuredMotoristBI.Other == null) splituninsuredMotoristBI.Other = uninsuredMotoristBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(splituninsuredMotoristBI.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId) splituninsuredMotoristBI.Other = uninsuredMotoristBIJson;

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                splituninsuredMotoristBI.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var txPIPJson = JsonConvert.SerializeObject(_mapper.Map<FormPIPDTO>(resource), JsonSerializerSettings);
                var pipTexas = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "TexasPersonalInjuryProtection");
                if (pipTexas != null)
                {
                    if (pipTexas.IsSelected == false) pipTexas.Other = null;
                    else
                    {
                        if (pipTexas.Other == null) pipTexas.Other = txPIPJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormPIPDTO>(pipTexas.Other);
                            if (entityOther.PIPLimitId != resource.PIPLimitId) pipTexas.Other = txPIPJson;

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                pipTexas.Other = JsonConvert.SerializeObject(_mapper.Map<FormPIPDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var txUMUIMJson = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(resource), JsonSerializerSettings);
                var txUMUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "TexasUMandUIMCoverage");
                if (txUMUIM != null)
                {
                    if (txUMUIM.IsSelected == false) txUMUIM.Other = null;
                    else
                    {
                        if (txUMUIM.Other == null) txUMUIM.Other = txUMUIMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(txUMUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId
                                || entityOther.UMBILimitId != resource.UMBILimitId
                                || entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                txUMUIM.Other = txUMUIMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                txUMUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var txSplitUMUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "TexasSplitUninsuredandUnderinsuredMotoristCoverage");
                if (txSplitUMUIM != null)
                {
                    if (txSplitUMUIM.IsSelected == false) txSplitUMUIM.Other = null;
                    else
                    {
                        if (txSplitUMUIM.Other == null) txSplitUMUIM.Other = txUMUIMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(txSplitUMUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId
                                || entityOther.UMBILimitId != resource.UMBILimitId
                                || entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                txSplitUMUIM.Other = txUMUIMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                txSplitUMUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var njPIPJson = JsonConvert.SerializeObject(_mapper.Map<FormPIPDTO>(resource), JsonSerializerSettings);
                var pipNJ = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "NewJerseyPersonalInjuryProtection");
                if (pipNJ != null)
                {
                    if (pipNJ.IsSelected == false) pipNJ.Other = null;
                    else
                    {
                        if (pipNJ.Other == null) pipNJ.Other = njPIPJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormPIPDTO>(pipNJ.Other);
                            if (entityOther.PIPLimitId != resource.PIPLimitId) pipNJ.Other = njPIPJson;

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                pipNJ.Other = JsonConvert.SerializeObject(_mapper.Map<FormPIPDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var njUMUIMJson = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(resource), JsonSerializerSettings);
                var njUMUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "NewJerseyUMandUIMCoverage");
                if (njUMUIM != null)
                {
                    if (njUMUIM.IsSelected == false) njUMUIM.Other = null;
                    else
                    {
                        if (njUMUIM.Other == null) njUMUIM.Other = njUMUIMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormNJUMUIMDTO>(njUMUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId
                                || entityOther.UIMPDLimitId != resource.UIMPDLimitId
                                || entityOther.UMBILimitId != resource.UMBILimitId
                                || entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                njUMUIM.Other = njUMUIMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                njUMUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormNJUMUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var njSplitUMUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "NewJerseySplitUninsuredandUnderinsuredMotoristCoverage");
                if (njSplitUMUIM != null)
                {
                    if (njSplitUMUIM.IsSelected == false) njSplitUMUIM.Other = null;
                    else
                    {
                        if (njSplitUMUIM.Other == null) njSplitUMUIM.Other = njUMUIMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormNJUMUIMDTO>(njSplitUMUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId
                                || entityOther.UMBILimitId != resource.UMBILimitId)
                            {
                                njSplitUMUIM.Other = njUMUIMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                njSplitUMUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormNJUMUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var moUMJson = JsonConvert.SerializeObject(_mapper.Map<FormUMDTO>(resource), JsonSerializerSettings);
                var moUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "MissouriUninsuredMotoristsCoverage");
                if (moUM != null)
                {
                    if (moUM.IsSelected == false) moUM.Other = null;
                    else
                    {
                        if (moUM.Other == null) moUM.Other = moUMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMDTO>(moUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId)
                            {
                                moUM.Other = moUMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                moUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var moUIMJson = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(resource), JsonSerializerSettings);
                var moUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "MissouriUnderinsuredMotoristsCoverage");
                if (moUIM != null)
                {
                    if (moUIM.IsSelected == false) moUIM.Other = null;
                    else
                    {
                        if (moUIM.Other == null) moUIM.Other = moUIMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(moUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                            {
                                moUIM.Other = moUIMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                moUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var paUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(resource), JsonSerializerSettings);
                var paUIMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(resource), JsonSerializerSettings);
                var paSplitUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "PennsylvaniaSplitUMCoverageLimitsNS");
                if (paSplitUM != null)
                {
                    if (paSplitUM.IsSelected == false) paSplitUM.Other = null;
                    else
                    {
                        if (paSplitUM.Other == null) paSplitUM.Other = paUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(paSplitUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId)
                            {
                                paSplitUM.Other = paUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                paSplitUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var paSplitUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "PennsylvaniaSplitUIMCoverageLimitsNS");
                if (paSplitUIM != null)
                {
                    if (paSplitUIM.IsSelected == false) paSplitUIM.Other = null;
                    else
                    {
                        if (paSplitUIM.Other == null) paSplitUIM.Other = paUIMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(paSplitUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                            {
                                paSplitUIM.Other = paUIMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                paSplitUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var paUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "PennsylvaniaUMCoverageLimitsNS");
                if (paUM != null)
                {
                    if (paUM.IsSelected == false) paUM.Other = null;
                    else
                    {
                        if (paUM.Other == null) paUM.Other = paUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(paUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId)
                            {
                                paUM.Other = paUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                paUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var paUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "PennsylvaniaUIMCoverageLimitsNS");
                if (paUIM != null)
                {
                    if (paUIM.IsSelected == false) paUIM.Other = null;
                    else
                    {
                        if (paUIM.Other == null) paUIM.Other = paUIMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(paUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                            {
                                paUIM.Other = paUIMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                paUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var flPIPJson = JsonConvert.SerializeObject(_mapper.Map<FormPIPDTO>(resource), JsonSerializerSettings);
                var pipFL = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "FloridaPersonalInjuryProtection");
                if (pipFL != null)
                {
                    if (pipFL.IsSelected == false) pipFL.Other = null;
                    else
                    {
                        if (pipFL.Other == null) pipFL.Other = flPIPJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormPIPDTO>(pipFL.Other);
                            if (entityOther.PIPLimitId != resource.PIPLimitId) pipFL.Other = flPIPJson;

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                pipFL.Other = JsonConvert.SerializeObject(_mapper.Map<FormPIPDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var flUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(resource), JsonSerializerSettings);
                var flUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "FloridaUninsuredMotoristsCoverageNS");
                if (flUM != null)
                {
                    if (flUM.IsSelected == false) flUM.Other = null;
                    else
                    {
                        if (flUM.Other == null) flUM.Other = flUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(flUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId)
                            {
                                flUM.Other = flUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                flUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var orPIPJson = JsonConvert.SerializeObject(_mapper.Map<FormPIPDTO>(resource), JsonSerializerSettings);
                var pipOR = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "OregonPersonalInjuryProtection");
                if (pipOR != null)
                {
                    if (pipOR.IsSelected == false) pipOR.Other = null;
                    else
                    {
                        if (pipOR.Other == null) pipOR.Other = orPIPJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormPIPDTO>(pipOR.Other);
                            if (entityOther.PIPLimitId != resource.PIPLimitId) pipOR.Other = orPIPJson;

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                pipOR.Other = JsonConvert.SerializeObject(_mapper.Map<FormPIPDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var orUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var orUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "OregonUninsuredMotoristCoverage");
                if (orUM != null)
                {
                    if (orUM.IsSelected == false) orUM.Other = null;
                    else
                    {
                        if (orUM.Other == null) orUM.Other = orUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(orUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                orUM.Other = orUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                orUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var ohUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(resource), JsonSerializerSettings);
                var ohUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "OhioUMandUIMCoverage");
                if (ohUM != null)
                {
                    if (ohUM.IsSelected == false) ohUM.Other = null;
                    else
                    {
                        if (ohUM.Other == null) ohUM.Other = ohUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(ohUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId ||
                                    entityOther.UIMBILimitId != resource.UIMBILimitId)
                            {
                                ohUM.Other = ohUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                ohUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var coUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var coUMBI = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "ColoradoUMBICoverage");
                if (coUMBI != null)
                {
                    if (coUMBI.IsSelected == false) coUMBI.Other = null;
                    else
                    {
                        if (coUMBI.Other == null) coUMBI.Other = coUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(coUMBI.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                coUMBI.Other = coUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                coUMBI.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var coUMPDJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var coUMPD = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "ColoradoUMPDCoverage");
                if (coUMPD != null)
                {
                    if (coUMPD.IsSelected == false) coUMPD.Other = null;
                    else
                    {
                        if (coUMPD.Other == null) coUMPD.Other = coUMPDJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(coUMPD.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                coUMPD.Other = coUMPDJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                coUMPD.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var coMPJson = JsonConvert.SerializeObject(_mapper.Map<FormMPDTO>(resource), JsonSerializerSettings);
                var coMP = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "ColoradoMedicalPaymentCoverage");
                if (coMP != null)
                {
                    if (coMP.IsSelected == false) coMP.Other = null;
                    else
                    {
                        if (coMP.Other == null) coMP.Other = coMPJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormMPDTO>(coMP.Other);
                            if (entityOther.MPLimitId != resource.MPLimitId)
                            {
                                coMP.Other = coMPJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                coMP.Other = JsonConvert.SerializeObject(_mapper.Map<FormMPDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }


                var inUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var inUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "IndianaUninsuredMotoristCoverage");
                if (inUM != null)
                {
                    if (inUM.IsSelected == false) inUM.Other = null;
                    else
                    {
                        if (inUM.Other == null) inUM.Other = inUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(inUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                inUM.Other = inUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                inUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var inUIMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(resource), JsonSerializerSettings);
                var inUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "IndianaUnderinsuredMotoristCoverage");
                if (inUIM != null)
                {
                    if (inUIM.IsSelected == false) inUIM.Other = null;
                    else
                    {
                        if (inUIM.Other == null) inUIM.Other = inUIMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(inUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                            {
                                inUIM.Other = inUIMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                inUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var vaUMJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var vaSplitUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "VirginiaSplitUMCoverage");
                if (vaSplitUM != null)
                {
                    if (vaSplitUM.IsSelected == false) vaSplitUM.Other = null;
                    else
                    {
                        if (vaSplitUM.Other == null) vaSplitUM.Other = vaUMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(vaSplitUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                vaSplitUM.Other = vaUMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                vaSplitUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var vaUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "VirginiaImportantNoticeUMCoverage");
                if (vaUM != null)
                {
                    if (vaUM.IsSelected == false) vaUM.Other = null;
                    else
                    {
                        if (vaUM.Other == null) vaUM.Other = vaUMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(vaUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                vaUM.Other = vaUMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                vaUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var tnUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var tnUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "TennesseeUninsuredMotoristCoverage");
                if (tnUM != null)
                {
                    if (tnUM.IsSelected == false) tnUM.Other = null;
                    else
                    {
                        if (tnUM.Other == null) tnUM.Other = tnUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(tnUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                tnUM.Other = tnUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                tnUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var ilUMJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var ilUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "IllinoisUMCoverage");
                if (ilUM != null)
                {
                    if (ilUM.IsSelected == false) ilUM.Other = null;
                    else
                    {
                        if (ilUM.Other == null) ilUM.Other = ilUMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(ilUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                ilUM.Other = ilUMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                ilUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var ilUMPDJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var ilUMPD = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "IllinoisUMPDCoverage");
                if (ilUMPD != null)
                {
                    if (ilUMPD.IsSelected == false) ilUMPD.Other = null;
                    else
                    {
                        if (ilUMPD.Other == null) ilUMPD.Other = ilUMPDJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(ilUMPD.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                ilUMPD.Other = ilUMPDJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                ilUMPD.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var ilUIMJson = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(resource), JsonSerializerSettings);
                var ilUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "IllinoisUIMCoverage");
                if (ilUIM != null)
                {
                    if (ilUIM.IsSelected == false) ilUIM.Other = null;
                    else
                    {
                        if (ilUIM.Other == null) ilUIM.Other = ilUIMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(ilUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                            {
                                ilUIM.Other = ilUIMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                ilUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var azUIMJson = JsonConvert.SerializeObject(_mapper.Map<FormUIMBIPDDTO>(resource), JsonSerializerSettings);
                var azUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "ArizonaUIMCoverage");
                if (azUIM != null)
                {
                    if (azUIM.IsSelected == false) azUIM.Other = null;
                    else
                    {
                        if (azUIM.Other == null) azUIM.Other = azUIMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIMBIPDDTO>(azUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId ||
                                entityOther.UIMPDLimitId != resource.UIMPDLimitId)
                            {
                                azUIM.Other = azUIMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                azUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var azUMJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var azUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "ArizonaUMCoverage");
                if (azUM != null)
                {
                    if (azUM.IsSelected == false) azUM.Other = null;
                    else
                    {
                        if (azUM.Other == null) azUM.Other = azUMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(azUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                azUM.Other = azUMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                azUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var msUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var msUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "MississippiUninsuredMotoristCoverageNS");
                if (msUM != null)
                {
                    if (msUM.IsSelected == false) msUM.Other = null;
                    else
                    {
                        if (msUM.Other == null) msUM.Other = msUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(msUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                msUM.Other = msUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                msUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var scSplitUMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(resource), JsonSerializerSettings);
                var scSplitUMBI = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "SouthCarolinaSplitUMCoverage");
                if (scSplitUMBI != null)
                {
                    if (scSplitUMBI.IsSelected == false) scSplitUMBI.Other = null;
                    else
                    {
                        if (scSplitUMBI.Other == null) scSplitUMBI.Other = scSplitUMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(scSplitUMBI.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId)
                            {
                                scSplitUMBI.Other = scSplitUMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                scSplitUMBI.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var scSplitUIMBIJson = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(resource), JsonSerializerSettings);
                var scSplitUIMBI = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "SouthCarolinaSplitUIMCoverage");
                if (scSplitUIMBI != null)
                {
                    if (scSplitUIMBI.IsSelected == false) scSplitUIMBI.Other = null;
                    else
                    {
                        if (scSplitUIMBI.Other == null) scSplitUIMBI.Other = scSplitUIMBIJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(scSplitUIMBI.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                            {
                                scSplitUIMBI.Other = scSplitUIMBIJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                scSplitUIMBI.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var scUMJson = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(resource), JsonSerializerSettings);
                var scUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "SouthCarolinaUMCoverage");
                if (scUM != null)
                {
                    if (scUM.IsSelected == false) scUM.Other = null;
                    else
                    {
                        if (scUM.Other == null) scUM.Other = scUMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(scUM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                scUM.Other = scUMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                scUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var scUIMJson = JsonConvert.SerializeObject(_mapper.Map<FormUIMBIPDDTO>(resource), JsonSerializerSettings);
                var scUIM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "SouthCarolinaUIMCoverage");
                if (scUIM != null)
                {
                    if (scUIM.IsSelected == false) scUIM.Other = null;
                    else
                    {
                        if (scUIM.Other == null) scUIM.Other = scUIMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormUIMBIPDDTO>(scUIM.Other);
                            if (entityOther.UIMBILimitId != resource.UIMBILimitId ||
                                entityOther.UIMPDLimitId != resource.UIMPDLimitId)
                            {
                                scUIM.Other = scUIMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                scUIM.Other = JsonConvert.SerializeObject(_mapper.Map<FormUIMBIPDDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var ticALJson = JsonConvert.SerializeObject(_mapper.Map<TrailerInterchangeALDTO>(resource), JsonSerializerSettings);
                var ticAL = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "TrailerInterchangeCoverageAL");
                if (ticAL != null && (policy.RaterVersionId >= 7 || policy.RaterVersionId == 0))
                {
                    if (ticAL.IsSelected == false) ticAL.Other = null;
                    else
                    {
                        if (ticAL.Other == null) ticAL.Other = ticALJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<TrailerInterchangeALDTO>(ticAL.Other);

                            if(entityOther.TrailerInterchangeLimitId != resource.TrailerInterchangeLimitId)
                            {
                                ticAL.Other = ticALJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                ticAL.Other = JsonConvert.SerializeObject(_mapper.Map<TrailerInterchangeALDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var pollutionCoverageJson = JsonConvert.SerializeObject(_mapper.Map<PollutionCoverageDTO>(resource), JsonSerializerSettings);
                var pollutionCoverage = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "PollutionCoverage");
                if (pollutionCoverage != null && (policy.RaterVersionId >= 7 || policy.RaterVersionId == 0))
                {
                    if (pollutionCoverage.IsSelected == false) pollutionCoverage.Other = null;
                    else
                    {
                        if (pollutionCoverage.Other == null) pollutionCoverage.Other = pollutionCoverageJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<PollutionCoverageDTO>(pollutionCoverage.Other);

                            if (entityOther.PollutionCovLimitId != resource.PollutionCovLimitId)
                            {
                                pollutionCoverage.Other = pollutionCoverageJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                pollutionCoverage.Other = JsonConvert.SerializeObject(_mapper.Map<PollutionCoverageDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                if (currentRiskDetailId.HasValue)
                {
                    var glJsonC = JsonConvert.SerializeObject(_mapper.Map<GeneralLiabilityDTO>(resource), JsonSerializerSettings);
                    var glEntityC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "GeneralLiabilityEndorsement");
                    if (glEntityC != null && hasGL2)
                    {
                        if (glEntityC.Other == null) glEntityC.Other = glJsonC;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<GeneralLiabilityDTO>(glEntityC.Other);
                            entityOther.UpdatedDate = resource.UpdatedDate;
                            glEntityC.Other = JsonConvert.SerializeObject(_mapper.Map<GeneralLiabilityDTO>(entityOther), JsonSerializerSettings);
                        }
                    }

                    var uiiaEntityC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "UIIAUpdated");
                    if (uiiaEntityC != null)
                    {
                        if (uiiaEntityC.IsSelected == false) uiiaEntityC.Other = null;
                        else
                        {
                            if (uiiaEntityC.Other == null) uiiaEntityC.Other = uiiaJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIIADTO>(uiiaEntityC.Other);
                                if (entityOther.AutoBILimitId != resource.AutoBILimitId) uiiaEntityC.Other = uiiaJson;
                            }
                        }
                    }

                    var cauninsuredMotoristBIC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "UninsuredMotoristCoverageBodilyInjury");
                    if (cauninsuredMotoristBIC != null)
                    {
                        if (cauninsuredMotoristBIC.IsSelected == false) cauninsuredMotoristBIC.Other = null;
                        else
                        {
                            if (cauninsuredMotoristBIC.Other == null) cauninsuredMotoristBIC.Other = uninsuredMotoristBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(cauninsuredMotoristBIC.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId) cauninsuredMotoristBIC.Other = uninsuredMotoristBIJson;
                            }
                        }
                    }

                    var splituninsuredMotoristBIC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "SplitBodilyInjuryUninsuredMotoristCoverage");
                    if (splituninsuredMotoristBIC != null)
                    {
                        if (splituninsuredMotoristBIC.IsSelected == false) splituninsuredMotoristBIC.Other = null;
                        else
                        {
                            if (splituninsuredMotoristBIC.Other == null) splituninsuredMotoristBIC.Other = uninsuredMotoristBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(splituninsuredMotoristBIC.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId) splituninsuredMotoristBIC.Other = uninsuredMotoristBIJson;
                            }
                        }
                    }

                    var ticAL2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "TrailerInterchangeCoverageAL");
                    if (ticAL2 != null && policy.RaterVersionId >= 7)
                    {
                        if (ticAL2.IsSelected == false) ticAL2.Other = null;
                        else
                        {
                            if (ticAL2.Other == null) ticAL2.Other = ticALJson;
                            else
                            {

                                var entityOther = JsonConvert.DeserializeObject<TrailerInterchangeALDTO>(ticAL2.Other);
                                if (entityOther.TrailerInterchangeLimitId != resource.TrailerInterchangeLimitId)
                                {
                                    ticAL2.Other = ticALJson;
                                }
                            }
                        }
                    }

                    var txPIP = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "TexasPersonalInjuryProtection");
                    if (txPIP != null)
                    {
                        if (txPIP.IsSelected == false) txPIP.Other = null;
                        else
                        {
                            if (txPIP.Other == null) txPIP.Other = txPIPJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormPIPDTO>(txPIP.Other);
                                if (entityOther.PIPLimitId != resource.PIPLimitId) txPIP.Other = txPIPJson;
                            }
                        }
                    }

                    var txUMUIMC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "TexasUMandUIMCoverage");
                    if (txUMUIMC != null)
                    {
                        if (txUMUIMC.IsSelected == false) txUMUIMC.Other = null;
                        else
                        {
                            if (txUMUIMC.Other == null) txUMUIMC.Other = txUMUIMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(txUMUIMC.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId
                                    || entityOther.UMBILimitId != resource.UMBILimitId
                                    || entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    txUMUIMC.Other = txUMUIMJson;
                                }
                            }
                        }
                    }

                    var txSplitUMUIMC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "TexasSplitUninsuredandUnderinsuredMotoristCoverage");
                    if (txSplitUMUIMC != null)
                    {
                        if (txSplitUMUIMC.IsSelected == false) txSplitUMUIMC.Other = null;
                        else
                        {
                            if (txSplitUMUIMC.Other == null) txSplitUMUIMC.Other = txUMUIMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(txSplitUMUIMC.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId
                                    || entityOther.UMBILimitId != resource.UMBILimitId
                                    || entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    txSplitUMUIMC.Other = txUMUIMJson;
                                }
                            }
                        }
                    }

                    var njPIP = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "NewJerseyPersonalInjuryProtection");
                    if (njPIP != null)
                    {
                        if (njPIP.IsSelected == false) njPIP.Other = null;
                        else
                        {
                            if (njPIP.Other == null) njPIP.Other = njPIPJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormPIPDTO>(njPIP.Other);
                                if (entityOther.PIPLimitId != resource.PIPLimitId) njPIP.Other = njPIPJson;
                            }
                        }
                    }

                    var njUMUIMC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "NewJerseyUMandUIMCoverage");
                    if (njUMUIMC != null)
                    {
                        if (njUMUIMC.IsSelected == false) njUMUIMC.Other = null;
                        else
                        {
                            if (njUMUIMC.Other == null) njUMUIMC.Other = njUMUIMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormNJUMUIMDTO>(njUMUIMC.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId
                                    || entityOther.UIMPDLimitId != resource.UIMPDLimitId
                                    || entityOther.UMBILimitId != resource.UMBILimitId
                                    || entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    njUMUIMC.Other = njUMUIMJson;
                                }
                            }
                        }
                    }

                    var njSplitUMUIMC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "NewJerseySplitUninsuredandUnderinsuredMotoristCoverage");
                    if (njSplitUMUIMC != null)
                    {
                        if (njSplitUMUIMC.IsSelected == false) njSplitUMUIMC.Other = null;
                        else
                        {
                            if (njSplitUMUIMC.Other == null) njSplitUMUIMC.Other = njUMUIMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormNJUMUIMDTO>(njSplitUMUIMC.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId
                                    || entityOther.UMBILimitId != resource.UMBILimitId)
                                {
                                    njSplitUMUIMC.Other = njUMUIMJson;
                                }
                            }
                        }
                    }

                    var moUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "MissouriUninsuredMotoristsCoverage");
                    if (moUM2 != null)
                    {
                        if (moUM2.IsSelected == false) moUM2.Other = null;
                        else
                        {
                            if (moUM2.Other == null) moUM2.Other = moUMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMDTO>(moUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId)
                                {
                                    moUM2.Other = moUMJson;
                                }
                            }
                        }
                    }

                    var moUIM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "MissouriUnderinsuredMotoristsCoverage");
                    if (moUIM2 != null)
                    {
                        if (moUIM2.IsSelected == false) moUIM2.Other = null;
                        else
                        {
                            if (moUIM2.Other == null) moUIM2.Other = moUIMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(moUIM2.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                                {
                                    moUIM2.Other = moUIMJson;
                                }
                            }
                        }
                    }

                    var paSplitUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "PennsylvaniaSplitUMCoverageLimitsNS");
                    if (paSplitUM2 != null)
                    {
                        if (paSplitUM2.IsSelected == false) paSplitUM2.Other = null;
                        else
                        {
                            if (paSplitUM2.Other == null) paSplitUM2.Other = paUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(paSplitUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId)
                                {
                                    paSplitUM2.Other = paUMBIJson;
                                }
                            }
                        }
                    }

                    var paSplitUIM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "PennsylvaniaSplitUIMCoverageLimitsNS");
                    if (paSplitUIM2 != null)
                    {
                        if (paSplitUIM2.IsSelected == false) paSplitUIM2.Other = null;
                        else
                        {
                            if (paSplitUIM2.Other == null) paSplitUIM2.Other = paUIMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(paSplitUIM2.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                                {
                                    paSplitUIM2.Other = paUIMBIJson;
                                }
                            }
                        }
                    }

                    var paUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "PennsylvaniaUMCoverageLimitsNS");
                    if (paUM2 != null)
                    {
                        if (paUM2.IsSelected == false) paUM2.Other = null;
                        else
                        {
                            if (paUM2.Other == null) paUM2.Other = paUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(paUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId)
                                {
                                    paUM2.Other = paUMBIJson;
                                }
                            }
                        }
                    }

                    var paUIM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "PennsylvaniaUIMCoverageLimitsNS");
                    if (paUIM2 != null)
                    {
                        if (paUIM2.IsSelected == false) paUIM2.Other = null;
                        else
                        {
                            if (paUIM2.Other == null) paUIM2.Other = paUIMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(paUIM2.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                                {
                                    paUIM2.Other = paUIMBIJson;
                                }
                            }
                        }
                    }

                    var pipFL2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "FloridaPersonalInjuryProtection");
                    if (pipFL2 != null)
                    {
                        if (pipFL2.IsSelected == false) pipFL2.Other = null;
                        else
                        {
                            if (pipFL2.Other == null) pipFL2.Other = flPIPJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormPIPDTO>(pipFL2.Other);
                                if (entityOther.PIPLimitId != resource.PIPLimitId) pipFL2.Other = flPIPJson;
                            }
                        }
                    }

                    var flUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "FloridaUninsuredMotoristsCoverageNS");
                    if (flUM2 != null)
                    {
                        if (flUM2.IsSelected == false) flUM2.Other = null;
                        else
                        {
                            if (flUM2.Other == null) flUM2.Other = flUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(flUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId)
                                {
                                    flUM2.Other = flUMBIJson;
                                }
                            }
                        }
                    }

                    var pipOR2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "OregonPersonalInjuryProtection");
                    if (pipOR2 != null)
                    {
                        if (pipOR2.IsSelected == false) pipOR2.Other = null;
                        else
                        {
                            if (pipOR2.Other == null) pipOR2.Other = orPIPJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormPIPDTO>(pipOR2.Other);
                                if (entityOther.PIPLimitId != resource.PIPLimitId) pipOR2.Other = orPIPJson;
                            }
                        }
                    }

                    var orUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "OregonUninsuredMotoristCoverage");
                    if (orUM2 != null)
                    {
                        if (orUM2.IsSelected == false) orUM2.Other = null;
                        else
                        {
                            if (orUM2.Other == null) orUM2.Other = orUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(orUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    orUM2.Other = orUMBIJson;
                                }
                            }
                        }
                    }

                    var ohUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "OhioUMandUIMCoverage");
                    if (ohUM2 != null)
                    {
                        if (ohUM2.IsSelected == false) ohUM2.Other = null;
                        else
                        {
                            if (ohUM2.Other == null) ohUM2.Other = ohUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(ohUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId ||
                                        entityOther.UIMBILimitId != resource.UIMBILimitId)
                                {
                                    ohUM2.Other = ohUMBIJson;
                                }
                            }
                        }
                    }

                    var coUMBI2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "ColoradoUMBICoverage");
                    if (coUMBI2 != null)
                    {
                        if (coUMBI2.IsSelected == false) coUMBI2.Other = null;
                        else
                        {
                            if (coUMBI2.Other == null) coUMBI2.Other = coUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(ohUM.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    coUMBI2.Other = coUMBIJson;
                                }
                            }
                        }
                    }

                    var inUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "IndianaUninsuredMotoristCoverage");
                    if (inUM2 != null)
                    {
                        if (inUM2.IsSelected == false) inUM2.Other = null;
                        else
                        {
                            if (inUM2.Other == null) inUM2.Other = inUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(inUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    inUM2.Other = inUMBIJson;
                                }
                            }
                        }
                    }

                    var coUMPD2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "ColoradoUMPDCoverage");
                    if (coUMPD2 != null)
                    {
                        if (coUMPD2.IsSelected == false) coUMPD2.Other = null;
                        else
                        {
                            if (coUMPD2.Other == null) coUMPD2.Other = coUMPDJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(coUMPD2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    coUMPD2.Other = coUMPDJson;
                                }
                            }
                        }
                    }

                    var coMP2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "ColoradoMedicalPaymentCoverage");
                    if (coMP2 != null)
                    {
                        if (coMP2.IsSelected == false) coMP2.Other = null;
                        else
                        {
                            if (coMP2.Other == null) coMP2.Other = coMPJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormMPDTO>(coMP2.Other);
                                if (entityOther.MPLimitId != resource.MPLimitId)
                                {
                                    coMP2.Other = coMPJson;
                                }
                            }
                        }
                    }

                    var inUIM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "IndianaUnderinsuredMotoristCoverage");
                    if (inUIM2 != null)
                    {
                        if (inUIM2.IsSelected == false) inUIM2.Other = null;
                        else
                        {
                            if (inUIM2.Other == null) inUIM2.Other = inUIMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(inUIM2.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                                {
                                    inUIM2.Other = inUIMBIJson;
                                }
                            }
                        }
                    }

                    var vaSplitUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "VirginiaSplitUMCoverage");
                    if (vaSplitUM2 != null)
                    {
                        if (vaSplitUM2.IsSelected == false) vaSplitUM2.Other = null;
                        else
                        {
                            if (vaSplitUM2.Other == null) vaSplitUM2.Other = vaUMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(vaSplitUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    vaSplitUM2.Other = vaUMJson;
                                }
                            }
                        }
                    }

                    var vaUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "VirginiaImportantNoticeUMCoverage");
                    if (vaUM2 != null)
                    {
                        if (vaUM2.IsSelected == false) vaUM2.Other = null;
                        else
                        {
                            if (vaUM2.Other == null) vaUM2.Other = vaUMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(vaUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    vaUM2.Other = vaUMJson;
                                }
                            }
                        }
                    }

                    var tnUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "TennesseeUninsuredMotoristCoverage");
                    if (tnUM2 != null)
                    {
                        if (tnUM2.IsSelected == false) tnUM2.Other = null;
                        else
                        {
                            if (tnUM2.Other == null) tnUM2.Other = tnUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(tnUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    tnUM2.Other = tnUMBIJson;
                                }
                            }
                        }
                    }

                    var ilUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "IllinoisUMCoverage");
                    if (ilUM2 != null)
                    {
                        if (ilUM2.IsSelected == false) ilUM2.Other = null;
                        else
                        {
                            if (ilUM2.Other == null) ilUM2.Other = ilUMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(ilUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    ilUM2.Other = ilUMJson;
                                }
                            }
                        }
                    }
                    var ilUMPD2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "IllinoisUMPDCoverage");
                    if (ilUMPD2 != null)
                    {
                        if (ilUMPD2.IsSelected == false) ilUMPD2.Other = null;
                        else
                        {
                            if (ilUMPD2.Other == null) ilUMPD2.Other = ilUMPDJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(ilUMPD2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    ilUMPD2.Other = ilUMPDJson;
                                }
                            }
                        }
                    }
                    var ilUIM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "IllinoisUIMCoverage");
                    if (ilUIM2 != null)
                    {
                        if (ilUIM2.IsSelected == false) ilUIM2.Other = null;
                        else
                        {
                            if (ilUIM2.Other == null) ilUIM2.Other = ilUIMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(ilUIM2.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                                {
                                    ilUIM2.Other = ilUIMJson;
                                }
                            }
                        }
                    }

                    var azUIM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "ArizonaUIMCoverage");
                    if (azUIM2 != null)
                    {
                        if (azUIM2.IsSelected == false) azUIM2.Other = null;
                        else
                        {
                            if (azUIM2.Other == null) azUIM2.Other = azUIMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIMBIPDDTO>(azUIM2.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId ||
                                    entityOther.UIMPDLimitId != resource.UIMPDLimitId)
                                {
                                    azUIM2.Other = azUIMJson;
                                }
                            }
                        }
                    }

                    var azUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "ArizonaUMCoverage");
                    if (azUM2 != null)
                    {
                        if (azUM2.IsSelected == false) azUM2.Other = null;
                        else
                        {
                            if (azUM2.Other == null) azUM2.Other = azUMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(azUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    azUM2.Other = azUMJson;
                                }
                            }
                        }
                    }

                    var msUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "MississippiUninsuredMotoristCoverageNS");
                    if (msUM2 != null)
                    {
                        if (msUM2.IsSelected == false) msUM2.Other = null;
                        else
                        {
                            if (msUM2.Other == null) msUM2.Other = msUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(msUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    msUM2.Other = msUMBIJson;
                                }
                            }
                        }
                    }

                    var scSplitUMBI2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "SouthCarolinaSplitUMCoverage");
                    if (scSplitUMBI2 != null)
                    {
                        if (scSplitUMBI2.IsSelected == false) scSplitUMBI2.Other = null;
                        else
                        {
                            if (scSplitUMBI2.Other == null) scSplitUMBI2.Other = scSplitUMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIDTO>(scSplitUMBI2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId)
                                {
                                    scSplitUMBI2.Other = scSplitUMBIJson;
                                }
                            }
                        }
                    }

                    var scSplitUIMBI2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "SouthCarolinaSplitUIMCoverage");
                    if (scSplitUIMBI2 != null)
                    {
                        if (scSplitUIMBI2.IsSelected == false) scSplitUIMBI2.Other = null;
                        else
                        {
                            if (scSplitUIMBI2.Other == null) scSplitUIMBI2.Other = scSplitUIMBIJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIMDTO>(scSplitUIMBI2.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId)
                                {
                                    scSplitUIMBI2.Other = scSplitUIMBIJson;
                                }
                            }
                        }
                    }

                    var scUM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "SouthCarolinaUMCoverage");
                    if (scUM2 != null)
                    {
                        if (scUM2.IsSelected == false) scUM2.Other = null;
                        else
                        {
                            if (scUM2.Other == null) scUM2.Other = scUMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUMBIPDDTO>(scUM2.Other);
                                if (entityOther.UMBILimitId != resource.UMBILimitId ||
                                    entityOther.UMPDLimitId != resource.UMPDLimitId)
                                {
                                    scUM2.Other = scUMJson;
                                }
                            }
                        }
                    }

                    var scUIM2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "SouthCarolinaUIMCoverage");
                    if (scUIM2 != null)
                    {
                        if (scUIM2.IsSelected == false) scUIM2.Other = null;
                        else
                        {
                            if (scUIM2.Other == null) scUIM2.Other = scUIMJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<FormUIMBIPDDTO>(scUIM2.Other);
                                if (entityOther.UIMBILimitId != resource.UIMBILimitId ||
                                    entityOther.UIMPDLimitId != resource.UIMPDLimitId)
                                {
                                    scUIM2.Other = scUIMJson;
                                }
                            }
                        }
                    }

                    var pollutionCoverage2 = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "PollutionCoverage");
                    if (pollutionCoverage2 != null && policy.RaterVersionId >= 7)
                    {
                        if (pollutionCoverage2.IsSelected == false) pollutionCoverage2.Other = null;
                        else
                        {
                            if (pollutionCoverage2.Other == null) pollutionCoverage2.Other = pollutionCoverageJson;
                            else
                            {
                                var entityOther = JsonConvert.DeserializeObject<PollutionCoverageDTO>(pollutionCoverage2.Other);

                                if (entityOther.PollutionCovLimitId != resource.PollutionCovLimitId)
                                {
                                    pollutionCoverage2.Other = pollutionCoverageJson;
                                }
                            }
                        }
                    }
                }

                var gaUMJson = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(resource), JsonSerializerSettings);
                var gaUM = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "GeorgiaPolicyHolderNoticeRegardingUMCoverage");
                if (gaUM != null)
                {
                    if (gaUM.IsSelected == false) gaUM.Other = null;
                    else
                    {
                        if (gaUM.Other == null) gaUM.Other = gaUMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(txUMUIM.Other);
                            if ( entityOther.UMBILimitId != resource.UMBILimitId
                                || entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                gaUM.Other = gaUMJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                gaUM.Other = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }

                var gaUMLiabJson = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(resource), JsonSerializerSettings);
                var gaUMLiab = await _riskFormRepository.FindAsync(e => e.RiskDetailId == resource.RiskDetailId && e.FormId == "GeorgiaUMCoverageReducedbyatFaultLiabilityLimits");
                if (gaUMLiab != null)
                {
                    if (gaUMLiab.IsSelected == false) gaUMLiab.Other = null;
                    else
                    {
                        if (gaUMLiab.Other == null) gaUMLiab.Other = gaUMJson;
                        else
                        {
                            var entityOther = JsonConvert.DeserializeObject<FormTXUMUIMDTO>(txUMUIM.Other);
                            if (entityOther.UMBILimitId != resource.UMBILimitId
                                || entityOther.UMPDLimitId != resource.UMPDLimitId)
                            {
                                gaUMLiab.Other = gaUMLiabJson;
                            }

                            if (isEffectiveDateChanged)
                            {
                                entityOther.UpdatedDate = (DateTime)resource.UpdatedDate;
                                gaUMLiab.Other = JsonConvert.SerializeObject(_mapper.Map<FormTXUMUIMDTO>(entityOther), JsonSerializerSettings);
                            }
                        }
                    }
                }
                // Driver
                //var firstRiskDetail = await _riskDetailRepository.GetFirstRiskDetailByRiskIdAsync(riskDetail.RiskId);
                //var firstRiskDetailForm = await _riskFormRepository.FindAsync(x => x.RiskDetailId == firstRiskDetail.Id && x.RiskDetailId != riskDetail.Id &&
                //                                x.Form.IsActive && x.IsSelected == true && x.FormId == "DriverExclusion");
                //if (firstRiskDetailForm != null && firstRiskDetailForm?.Other != null)
                //{
                //    var entityOther = JsonConvert.DeserializeObject<FormDriverExclusionDTO>(firstRiskDetailForm.Other);
                //    resource.Drivers.AddRange(entityOther.Drivers);
                //}

                //var hasDriverExclusionAdded = false;
                //var driverJson = JsonConvert.SerializeObject(_mapper.Map<FormDriverExclusionDTO>(resource), JsonSerializerSettings);
                //if (resource.Drivers.Any() && driverEntity != null)
                //{
                //    if (driverEntity?.Other != null)
                //    {
                //        var excludedDrivers = JsonConvert.DeserializeObject<FormDriverExclusionDTO>(driverEntity.Other);
                //        foreach (var item in resource.Drivers)
                //        {
                //            if (excludedDrivers.Drivers.Any(x => x.LicenseNumber == item.LicenseNumber) == false)
                //                hasDriverExclusionAdded = true;
                //        }
                //    }
                //    else
                //        hasDriverExclusionAdded = true;

                //    if (hasDriverExclusionAdded)
                //        driverEntity.Other = driverJson;
                //}
                //if (driverEntity.Other is null)
                //    driverEntity.IsSelected = false;

                //if (currentRiskDetailId.HasValue)
                //{
                //    var driverEntityC = await _riskFormRepository.FindAsync(e => e.RiskDetailId == currentRiskDetailId && e.FormId == "DriverExclusion");
                //    if (resource.Drivers.Any() && driverEntityC != null)
                //    {
                //        if (hasDriverExclusionAdded)
                //            driverEntityC.Other = driverJson;
                //    }
                //}

                if(uiiaEntity != null)
                    _riskFormRepository.Update(uiiaEntity);

                if (trailerEntity != null)
                    _riskFormRepository.Update(trailerEntity);

                if (territoryEntity != null)
                    _riskFormRepository.Update(territoryEntity);

                if (pdLimitEntity != null)
                    _riskFormRepository.Update(pdLimitEntity);

                if (occupantHazard != null)
                    _riskFormRepository.Update(occupantHazard);

                if (driverEntity != null)
                    _riskFormRepository.Update(driverEntity);

                if (cauninsuredMotoristBI != null)
                    _riskFormRepository.Update(cauninsuredMotoristBI);

                if (splituninsuredMotoristBI != null)
                    _riskFormRepository.Update(splituninsuredMotoristBI);

                if (pipTexas != null)
                    _riskFormRepository.Update(pipTexas);

                if (txUMUIM != null)
                    _riskFormRepository.Update(txUMUIM);

                if (txSplitUMUIM != null)
                    _riskFormRepository.Update(txSplitUMUIM);

                if (pipNJ != null)
                    _riskFormRepository.Update(pipNJ);

                if (njUMUIM != null)
                    _riskFormRepository.Update(njUMUIM);

                if (njSplitUMUIM != null)
                    _riskFormRepository.Update(njSplitUMUIM);

                if (moUM != null)
                    _riskFormRepository.Update(moUM);

                if (moUIM != null)
                    _riskFormRepository.Update(moUIM);

                if (paSplitUM != null)
                    _riskFormRepository.Update(paSplitUM);

                if (paSplitUIM != null)
                    _riskFormRepository.Update(paSplitUIM);

                if (paUM != null)
                    _riskFormRepository.Update(paUM);

                if (paUIM != null)
                    _riskFormRepository.Update(paUIM);

                if (pipFL != null)
                    _riskFormRepository.Update(pipFL);

                if (flUM != null)
                    _riskFormRepository.Update(flUM);

                if (pipOR != null)
                    _riskFormRepository.Update(pipOR);

                if (orUM != null)
                    _riskFormRepository.Update(orUM);

                if (ohUM != null)
                    _riskFormRepository.Update(ohUM);

                if (coUMBI != null)
                    _riskFormRepository.Update(coUMBI);

                if (coUMPD != null)
                    _riskFormRepository.Update(coUMPD);

                if (coMP != null)
                    _riskFormRepository.Update(coMP);

                if (inUM != null)
                    _riskFormRepository.Update(inUM);

                if (inUM != null)
                    _riskFormRepository.Update(inUM);

                if (vaSplitUM != null)
                    _riskFormRepository.Update(vaSplitUM);

                if (vaUM != null)
                    _riskFormRepository.Update(vaUM);

                if (tnUM != null)
                    _riskFormRepository.Update(tnUM);

                if (ilUM != null)
                    _riskFormRepository.Update(ilUM);

                if (ilUMPD != null)
                    _riskFormRepository.Update(ilUMPD);

                if (ilUIM != null)
                    _riskFormRepository.Update(ilUIM);

                if (azUM != null)
                    _riskFormRepository.Update(azUM);

                if (azUIM != null)
                    _riskFormRepository.Update(azUIM);

                if (scSplitUMBI != null)
                    _riskFormRepository.Update(scSplitUMBI);

                if (scSplitUIMBI != null)
                    _riskFormRepository.Update(scSplitUIMBI);

                if (scUM != null)
                    _riskFormRepository.Update(scUM);

                if (scUIM != null)
                    _riskFormRepository.Update(scUIM);

                await _riskFormRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
            }

        }

        private void SetUpdatedDate<T>(RiskForm entity, RiskFormOtherDTO resource, bool hasChanges = false) where T : IDialogUpdate
        {
            if (entity == null || (!entity.IsSelected.GetValueOrDefault() && !hasChanges)) return;
            if (!entity.IsSelected.GetValueOrDefault() && hasChanges) entity.Other = null;
            else
            {
                var otherJson = JsonConvert.SerializeObject(_mapper.Map<T>(resource), JsonSerializerSettings);
                if (entity.Other == null) entity.Other = otherJson;
                else
                {
                    var entityOther = JsonConvert.DeserializeObject<T>(entity.Other);
                    if (entityOther.UpdatedDate != null && !hasChanges) return;
                    entityOther.UpdatedDate = resource.UpdatedDate;
                    entity.Other = JsonConvert.SerializeObject(_mapper.Map<T>(entityOther), JsonSerializerSettings);
                }
            }
        }

        private async Task<List<string>> GetUserStates(Guid id)
        {
            var userStates = new List<string>();

            var nameAddress = await _namedAddressService.GetNameAndAddress(id);

            userStates.Add(nameAddress.State);
            userStates.Add(nameAddress.StateOperations);

            var garagingAddress = nameAddress.Addresses.Where(na => na.AddressTypeId == 3).Select(na => na.Address?.State);
            userStates.AddRange(garagingAddress);

            return userStates;
        }

        public async Task<bool> RemoveFormAsync(Guid id)
        {
            var riskForm = await _riskFormRepository.FindAsync(e => e.Id == id);
            var form = await _formRepository.FindAsync(e => e.Id == riskForm.FormId);
            riskForm.IsActive = false;
            riskForm.DeletedDate = DateTime.Now;
            riskForm.IsSelected = false;
            var result = _riskFormRepository.Update(riskForm);
            await _riskFormRepository.SaveChangesAsync();

            form.IsActive = false;
            _formRepository.Update(form);
            await _formRepository.SaveChangesAsync();

            return true;
        }

        private static bool IsDriverDeletedOrInKoRule(DriverDTO driver, DateTime brokerEffectiveDate, ICollection<Guid> previousVersionIds)
        {
            var endDate = brokerEffectiveDate.Date;
            var startDate = endDate.AddMonths(-36).Date;

            if (driver.DeletedDate != null && !previousVersionIds.Contains(driver.Id.GetValueOrDefault())) return true;

            var isAgeStringValid = int.TryParse(driver.Age, out var driverAge);
            if (!isAgeStringValid) return false;
            if (driverAge <= 22) return true;
            if ((driverAge == 23 || driverAge == 24) && driver.IsMvr == false && driver.isExcludeKO == false) return true;

            var koIncidentType = new[] {"1", "2", "4", "5", "8"};
            var koIncidents = driver.DriverIncidents.Where(di => koIncidentType.Contains(di.IncidentType)
                                                                 && di.IncidentDate <= endDate &&
                                                                 di.IncidentDate >= startDate
                                                                 && driver.isExcludeKO == false).ToList();

            return koIncidents.Count > 1;
        }

        public async Task<RiskFormDTO> RiskFormUpdateAsync(RiskFormDTO riskForm)
        {
            var entity = await _riskFormRepository.FindAsync(e => e.RiskDetailId == riskForm.RiskDetailId && e.FormId == riskForm.FormId);

            if (entity != null)
            {
                entity.IsSelected = riskForm.IsSelected;
                entity.Other = riskForm.Other;
                _riskFormRepository.Update(entity);

                if (riskForm.IsSelected == false)
                {
                    switch (entity.FormId)
                    {
                        case "HiredPhysicalDamage":
                            await _quoteOptionsService.RemoveHiredPhysicalDamage(riskForm.RiskDetailId);
                            break;
                        case "TrailerInterchangeCoverage":
                            await _quoteOptionsService.RemoveTrailerInterchange(riskForm.RiskDetailId);
                            break;
                        default:
                            break;
                    }
                }
                await _riskFormRepository.SaveChangesAsync();
            }
            return riskForm;
        }

        public async Task<List<SymbolTenResponseDTO>> GetSymbolTenResponses(Guid riskDetailId)
        {
            var symbolTenResponses = await _context.SymbolTenResponse.AsNoTracking().ToListAsync();
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var riskCoverage = riskDetail?.RiskCoverages[0];

            if(riskCoverage != null)
            {
                foreach(var response in symbolTenResponses)
                {
                    if(response.Id != 6)
                        response.Description = response.Description.Replace("{VAL}", riskCoverage.ALSymbolId == 14 ? "2" : "7");
                }
            }

            return _mapper.Map<List<SymbolTenResponseDTO>>(symbolTenResponses);
        }

        public async Task<List<RiskFormDTO>> InsertDefaultsNewAsync(Guid riskId)
        {
            var forms = await _formRepository.GetAsync(f => f.IsActive && !f.IsSupplementalDocument);
            var existingRiskForms = await _riskFormRepository.GetAsync(r => r.RiskDetailId == riskId);
            var userStates = await GetUserStates(riskId);
            var baseForms = new List<RiskForm>();
            var symbolTenResponses = await GetSymbolTenResponses(riskId);
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskId);
            var riskCoverage = riskDetail.RiskCoverages[0];
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);

            foreach (var form in forms)
            {
                if (existingRiskForms.Any(e => e.FormId == form.Id))
                    continue;

                var riskForm = new RiskForm
                {
                    Id = Guid.NewGuid(),
                    RiskDetailId = riskId,
                    FormId = form.Id,
                    CreatedBy = 0,
                    CreatedDate = DateTime.Now,
                    Form = form,
                    IsSelected = form.IsDefault
                };

                riskForm.Other = form.Id switch
                {
                    "GeneralLiabilityEndorsement" => JsonConvert.SerializeObject(
                        new GeneralLiabilityDTO { InjuryLimit = 1000000, MedicalExpenses = 5000, DamagesRentedToYou = 100000, AggregateLimit = 2000000 }, JsonSerializerSettings),
                    "ExclusionRadiusEndorsement" => JsonConvert.SerializeObject(
                        new ExclusionRadiusDTO { Radius = 300 }, JsonSerializerSettings),
                    "TerritoryExclusion" => JsonConvert.SerializeObject(
                        new TerritoryExclusionDTO { States = string.Join(",", userStates.Distinct()), StatesDefault = true }, JsonSerializerSettings),
                    "DriverRequirements" => JsonConvert.SerializeObject(
                        new DriverRequirementDTO
                        {
                            Requirement = "No more than one (1) moving violation or at-fault accident in the last thirty-six (36) months"
                        }, JsonSerializerSettings),
                    "CoveredAutoDesignationSymbol" => JsonConvert.SerializeObject(
                        new CoveredAutoDesigSymbolDTO { CoveredAutoText = symbolTenResponses?.FirstOrDefault()?.Id.ToString(), TextForEndorsement = null }, JsonSerializerSettings),
                    _ => riskForm.Other
                };

                if (form.Id == "CoveredAutoDesignationSymbol" && (riskCoverage?.ALSymbolId == 14 || riskCoverage?.ALSymbolId == 15))
                    riskForm.IsSelected = true;

                if (form.Id == "FormFMotorCarrierBIPD" && (riskDetail?.RiskFilings?.Any(rf => rf.FilingTypeId == 9) ?? false))
                    riskForm.IsSelected = true;

                if (form.Id == "MotorCarrierEndorsement" && (riskDetail?.IsInsuredHasMotorCarrierNo ?? false))
                    riskForm.IsSelected = true;

                baseForms.Add(riskForm);
            }

            baseForms.ForEach(rf => rf.Form = null);
            await _riskFormRepository.AddRangeAsync(baseForms);
            await _riskFormRepository.SaveChangesAsync();
            var result = await _riskFormRepository.GetByIdIncludeAsync(riskId);

            //Update displayed form number depending on version
            foreach (var riskForm in result)
            {
                var formVersion = riskForm.Form.FormVersions?.FirstOrDefault(fv => riskDetail.RequestedQuoteDate > fv.EffectiveDate);

                if (riskDetail.RequestedQuoteDate == null && !risk.IsRaterVersionLocked)
                    formVersion = riskForm.Form.FormVersions?.OrderByDescending(fv => fv.Version)?.FirstOrDefault();

                if (formVersion != null)
                    riskForm.Form.FormNumber = formVersion.FormNumber;
            }

            var riskFormsResult = _mapper.Map<List<RiskFormDTO>>(result);
            await MergeManuscript(riskFormsResult, riskId, riskDetail.RequestedQuoteDate, risk.IsRaterVersionLocked);

            return riskFormsResult;
        }

        public async Task MergeManuscript(List<RiskFormDTO> riskForms, Guid riskId, DateTime? quoteDate, bool isRaterVersionLocked)
        {
            if (!riskForms.Any())
                return;

            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskId);
            var manuscriptForm = _mapper.Map<List<FormDTO>>(await _formRepository.GetAsync(f => f.Id == "ManuscriptEndorsementFullEarn", entity => entity.FormVersions)).FirstOrDefault();
            var formNumber = "RPCAMANU";
            if (manuscripts != null)
            {
                var ofacSortOrder = riskForms.Where(x => x.FormId == "OFACNotice").FirstOrDefault()?.Form?.SortOrder;
                if (ofacSortOrder == null)
                    return;

                foreach (var manu in manuscripts)
                {
                    if (manuscriptForm != null)
                    {
                        var formVersion = manuscriptForm?.FormVersions?.Where(fv => quoteDate >= fv.EffectiveDate)?
                                .OrderBy(fv => fv.EffectiveDate)?
                                .FirstOrDefault() ??
                                manuscriptForm?.FormVersions?
                                .OrderByDescending(fv => fv.EffectiveDate)?
                                .FirstOrDefault();

                        if (formVersion != null && (quoteDate != null || !isRaterVersionLocked))
                            formNumber = formVersion.FormNumber;
                    }

                    if (manu.IsProrate == false && manu.DeletedDate != null) continue;
                    riskForms.Add(new RiskFormDTO()
                    {
                        Id = manu.Id,
                        IsSelected = manu.IsSelected,
                        RiskDetailId = manu.RiskDetailId,
                        FormId = "ManuscriptEndorsement",
                        Form = new FormDTO()
                        {
                            FormNumber = formNumber,
                            Id = "ManuscriptEndorsement",
                            Name = manu.Title,
                            SortOrder = (int)ofacSortOrder - 1
                        }
                    });
                }
            }
        }

        private async Task<bool> IsFeatureApplicable(Guid RiskDetailId, string fieldName)
        {
            var fieldVersions = await _riskRepository.GetFieldVersions();
            var submissionRiskDetail = await _riskDetailRepository.GetFirstRiskDetailByRiskIdAsync(RiskDetailId);
            var risk = await _riskRepository.FindAsync(submissionRiskDetail.RiskId);
            var isApplicable = fieldVersions.Any(v => v.FieldName == fieldName && submissionRiskDetail.RequestedQuoteDate >= v.EffectiveDate);

            if (submissionRiskDetail.RequestedQuoteDate == null && !risk.IsRaterVersionLocked)
                return fieldVersions.Any(v => v.FieldName == fieldName);

            return isApplicable;
        }
    }
}