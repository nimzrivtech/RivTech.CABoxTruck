using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class CoverageLimitService : ICoverageLimitService
    {
        private readonly IFormRepository _formRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly ICoverageHistoryService _coverageHistoryService;
        private readonly IRiskFormRepository _riskFormRepository;
        private readonly ILvLimitsRepository _lvLimitsRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IFormsService _formsService;
        private readonly IMapper _mapper;
        private Int16 optionNumber = 1;
        private List<int> _UMBISingleLimits = new List<int> { 314, 318, 319, 320, 321, 322 };
        private List<int> _UMBISplitLimits = new List<int> { 310, 311, 312, 313, 315, 316, 317 };
        private List<int> _TXPIPLimits = new List<int> { 147, 148, 149, 150, 151 };
        private List<int> _TXSplitUMUIMLimits = new List<int> { 278,279,280,282,434,435,436,438 };
        private List<int> _NJSplitUIMBILimits = new List<int> { 231, 232, 233, 235, 237, 238 };
        private List<int> _NJSplitUMBILimits = new List<int> { 376, 377, 378, 380, 382, 383 };
        private List<int> _GAUMLimits = new List<int> { 334, 335, 336, 337, 338, 339, 340, 341, 342, 343 };
        private List<int> _PASplitUMBILimits = new List<int> { 410,411,413,414,416 };
        private List<int> _PASplitUIMBILimits = new List<int> { 265,266,268,269,271 };
        private List<int> _VASplitUMBILimits = new List<int> { 665, 666, 667, 668 };
        private List<int> _SCSplitUMBILimits = new List<int> { 794, 795, 796, 800, 803, 808, 812, 814 };
        private List<int> _SCSplitUIMBILimits = new List<int> { 760, 761, 762, 766, 769, 774, 778, 780 };

        public CoverageLimitService(IFormRepository formRepository,
            IRiskCoverageRepository riskCoverageRepositoryy,
            ICoverageHistoryService coverageHistoryService,
            IRiskFormRepository riskFormRepository,
            ILvLimitsRepository lvLimitsRepository,
            IRiskDetailRepository riskDetailRepository,
            IRiskRepository riskRepository,
            IFormsService formsService,
            IMapper mapper)
        {
            _formRepository = formRepository;
            _riskCoverageRepository = riskCoverageRepositoryy;
            _coverageHistoryService = coverageHistoryService;
            _riskFormRepository = riskFormRepository;
            _lvLimitsRepository = lvLimitsRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskRepository = riskRepository;
            _formsService = formsService;
            _mapper = mapper;
        }

        public JsonSerializerSettings JsonSerializerSettings =>
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new DecimalFormatConverter() }
            };

        public async Task<List<RiskCoverageDTO>> GetAllAsync()
        {
            var result = await _riskCoverageRepository.GetAllAsync();
            return _mapper.Map<List<RiskCoverageDTO>>(result);
        }

        public async Task<RiskCoverageDTO> GetByIdAsync(Guid id)
        {
            var result = await _riskCoverageRepository.FindAsync(x => x.RiskDetailId == id && x.OptionNumber== 0);
            return _mapper.Map<RiskCoverageDTO>(result);
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _riskCoverageRepository.AnyAsync(x => x.RiskDetailId == id);
        }

        public async Task<SaveRiskCoverageDTO> InsertAsync(SaveRiskCoverageDTO model)
        {
            model.OptionNumber = this.optionNumber;
            var result = await _riskCoverageRepository.AddAsync(_mapper.Map<RiskCoverage>(model));
            await _riskCoverageRepository.SaveChangesAsync();
            return model;
        }

        public async Task<SaveRiskCoverageDTO> UpdateAsync(SaveRiskCoverageDTO model)
        {
            var data = await _riskCoverageRepository.FindAsync(x => x.RiskDetailId == model.RiskDetailId && x.OptionNumber == model.OptionNumber);
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(data.RiskDetailId);
            var risk = await _riskRepository.GetRiskByIdAsync(riskDetail.RiskId);
            var raterVersion = risk.RaterVersionId;
            var oldAlSymbol = data.ALSymbolId;
            var symbolTenResponses = await _formsService.GetSymbolTenResponses(data.RiskDetailId);
            var hasTrailerType = ((model.TrailerInterchangeLimitId != null && model.TrailerInterchangeLimitId != LimitConstants.TrailerNoCoverageId) ||
                                  (model.NonOwnedTrailerLimitId != null && model.NonOwnedTrailerLimitId != LimitConstants.NonOwnedTrailerNoCoverageId));

            data.AutoBILimitId = model.AutoBILimitId;
            data.AutoPDLimitId = model.AutoPDLimitId;
            data.LiabilityDeductibleId = model.LiabilityDeductibleId;
            data.MedPayLimitId = model.MedPayLimitId;
            data.PIPLimitId = model.PIPLimitId;
            data.UMBILimitId = model.UMBILimitId;
            data.UMPDLimitId = model.UMPDLimitId;
            data.UIMBILimitId = model.UIMBILimitId;
            data.UIMPDLimitId = model.UIMPDLimitId;
            data.CoveredSymbolsId = model.CoveredSymbolsId;
            data.SpecifiedPerils = model.SpecifiedPerils;
            data.CompDeductibleId = model.CompDeductibleId;
            data.FireDeductibleId = model.FireDeductibleId;
            data.CollDeductibleId = model.CollDeductibleId;
            data.GLBILimitId = model.GLBILimitId;
            data.CargoLimitId = model.CargoLimitId;
            data.RefCargoLimitId = model.RefCargoLimitId;
            data.TrailerInterchangeLimitId = model.TrailerInterchangeLimitId;
            data.NonOwnedTrailerLimitId = model.NonOwnedTrailerLimitId;
            data.TrailerTypeLimitId = hasTrailerType ? model.TrailerTypeLimitId : null;
            data.PollutionCovLimitId = model.PollutionCovLimitId;

            await _coverageHistoryService.DeleteFieldsAsync(model.RiskDetailId,
                    false,
                    false,
                    (!data.CompDeductibleId.HasValue || data.CompDeductibleId.Value == 1) && (!data.FireDeductibleId.HasValue || data.FireDeductibleId.Value == 1),
                    !data.CollDeductibleId.HasValue || data.CollDeductibleId.Value == 1,
                    !data.CargoLimitId.HasValue || data.CargoLimitId.Value == 1,
                    !data.RefCargoLimitId.HasValue || data.RefCargoLimitId.Value == 1
                );

            data.ALSymbolId = model.ALSymbolId;
            data.PDSymbolId = model.PDSymbolId;
            data.LiabilityDeductibleSymbolId = model.LiabilityDeductibleSymbolId;
            data.MedPayLimitSymbolId = model.MedPayLimitSymbolId;
            data.PIPLimitSymbolId = model.PIPLimitSymbolId;
            data.UMBILimitSymbolId = model.UMBILimitSymbolId;
            data.UMPDLimitSymbolId = model.UMPDLimitSymbolId;
            data.UIMBILimitSymbolId = model.UIMBILimitSymbolId;
            data.UIMPDLimitSymbolId = model.UIMPDLimitSymbolId;
            data.CompDeductibleSymbolId = model.CompDeductibleSymbolId;
            data.FireDeductibleSymbolId = model.FireDeductibleSymbolId;
            data.CollDeductibleSymbolId = model.CollDeductibleSymbolId;
            data.GLBILimitSymbolId = model.GLBILimitSymbolId;
            data.CargoLimitSymbolId = model.CargoLimitSymbolId;
            data.RefCargoLimitSymbolId = model.RefCargoLimitSymbolId;
            data.TrailerInterchangeLimitSymbolId = model.TrailerInterchangeLimitSymbolId;
            data.NonOwnedTrailerLimitSymbolId = model.NonOwnedTrailerLimitSymbolId;

            data.EndorsementNumber = model.EndorsementNumber;
            data.OptionNumber = model.OptionNumber;
            data.NumAddtlInsured = model.NumAddtlInsured;
            data.WaiverSubrogation = model.WaiverSubrogation;
            data.PrimeNonContribEndorsement = model.PrimeNonContribEndorsement;

            var result = _riskCoverageRepository.Update(data);
            await _riskCoverageRepository.SaveChangesAsync();

            // Update UMBI form
            var umbiform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId &&
                                (x.FormId == "UninsuredMotoristCoverageBodilyInjury" || x.FormId == "SplitBodilyInjuryUninsuredMotoristCoverage"));
            if (umbiform != null)
            {
                foreach(var form in umbiform)
                {
                    form.IsSelected = false;
                    if (_UMBISingleLimits.Any(x => x == result.UMBILimitId) || _UMBISplitLimits.Any(x => x == result.UMBILimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update RefCargo form
            var refcargoform = await _riskFormRepository.FindAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "RefrigerationCargoEndorsement");
            if (refcargoform != null)
            {
                refcargoform.IsSelected = false;
                if (data.RefCargoLimitId.HasValue && data.RefCargoLimitId.Value != 157)
                    refcargoform.IsSelected = true;
                _riskFormRepository.Update(refcargoform);
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update GL form
            var glForm = await _riskFormRepository.FindAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "GeneralLiabilityEndorsement");
            if (glForm != null)
            {
                glForm.IsSelected = false;
                if (data.GLBILimitId.HasValue && data.GLBILimitId.Value != 87)
                    glForm.IsSelected = true;
                _riskFormRepository.Update(glForm);
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update Trailer Interchange Coverage AL form
            var ticALForm = await _riskFormRepository.FindAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "TrailerInterchangeCoverageAL");
            if (ticALForm != null)
            {
                ticALForm.IsSelected = false;
                if (raterVersion >= 7 && data.TrailerInterchangeLimitId != null && data.TrailerInterchangeLimitId != 822)
                    ticALForm.IsSelected = true;
                _riskFormRepository.Update(ticALForm);
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update TX PIP forms
            var txform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "TexasPersonalInjuryProtection");
            if (txform != null)
            {
                foreach (var form in txform)
                {
                    form.IsSelected = false;
                    if (_TXPIPLimits.Any(x => x == result.PIPLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update TX UMUIM forms
            var txUMUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "TexasUMandUIMCoverage");
            if (txUMUIMform != null)
            {
                foreach (var form in txUMUIMform)
                {
                    form.IsSelected = false;
                    if ((result.UIMBILimitId >= 278 && result.UIMBILimitId <= 286)
                        || (result.UMBILimitId >= 434 && result.UMBILimitId <= 442)
                        || (result.UMPDLimitId >= 532 && result.UMPDLimitId <= 538)
                        )
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update TX Split UMUIM forms
            var txSplitUMUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "TexasSplitUninsuredandUnderinsuredMotoristCoverage");
            if (txSplitUMUIMform != null)
            {
                foreach (var form in txSplitUMUIMform)
                {
                    form.IsSelected = false;
                    if (_TXSplitUMUIMLimits.Any(x => x == result.UIMBILimitId) ||
                        _TXSplitUMUIMLimits.Any(x => x == result.UMBILimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update NJ PIP forms
            var njform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "NewJerseyPersonalInjuryProtection");
            if (njform != null)
            {
                foreach (var form in njform)
                {
                    form.IsSelected = false;
                    if (result.PIPLimitId >= 119 && result.PIPLimitId <= 129)
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update TX UMUIM forms
            var njUMUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "NewJerseyUMandUIMCoverage");
            if (njUMUIMform != null)
            {
                foreach (var form in njUMUIMform)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 376 && result.UMBILimitId <= 387)
                        || (result.UMPDLimitId >= 499 && result.UMPDLimitId <= 508)
                        || (result.UIMBILimitId >= 231 && result.UIMBILimitId <= 242)
                        || (result.UIMPDLimitId >= 288 && result.UIMPDLimitId <= 297)
                        )
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update TX Split UMUIM forms
            var njSplitUMUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "NewJerseySplitUninsuredandUnderinsuredMotoristCoverage");
            if (njSplitUMUIMform != null)
            {
                foreach (var form in njSplitUMUIMform)
                {
                    form.IsSelected = false;
                    if (_NJSplitUIMBILimits.Any(x => x == result.UIMBILimitId) ||
                        _NJSplitUMBILimits.Any(x => x == result.UMBILimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update MO UM forms
            var moUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "MissouriUninsuredMotoristsCoverage");
            if (moUMform != null)
            {
                foreach (var form in moUMform)
                {
                    form.IsSelected = false;
                    if (result.UMBILimitId >= 365 && result.UMBILimitId <= 374)
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update MO UIM forms
            var moUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "MissouriUnderinsuredMotoristsCoverage");
            if (moUIMform != null)
            {
                foreach (var form in moUIMform)
                {
                    form.IsSelected = false;
                    if (result.UIMBILimitId >= 221 && result.UIMBILimitId <= 229)
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update GA UM forms
            var gaUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "GeorgiaPolicyHolderNoticeRegardingUMCoverage");
            if (gaUMform != null)
            {
                foreach (var form in gaUMform)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 334 && result.UMBILimitId <= 343)
                        || (result.UMPDLimitId >= 449 && result.UMPDLimitId <= 472)
                        )
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update GA UM forms
            var gaUMLiabform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "GeorgiaUMCoverageReducedbyatFaultLiabilityLimits");
            if (gaUMLiabform != null)
            {
                foreach (var form in gaUMLiabform)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 334 && result.UMBILimitId <= 343)
                        || (result.UMPDLimitId >= 449 && result.UMPDLimitId <= 472)
                        )
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update PA Split UM forms
            var paSplitUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "PennsylvaniaSplitUMCoverageLimitsNS");
            if (paSplitUMform != null)
            {
                foreach (var form in paSplitUMform)
                {
                    form.IsSelected = false;
                    if (result.UMBILimitId >= 410 && result.UMBILimitId <= 421)
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update PA Split UIM forms
            var paSplitUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "PennsylvaniaSplitUIMCoverageLimitsNS");
            if (paSplitUIMform != null)
            {
                foreach (var form in paSplitUIMform)
                {
                    form.IsSelected = false;
                    if (result.UIMBILimitId >= 265 && result.UIMBILimitId <= 276)
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update FL UM forms
            var flUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "FloridaUninsuredMotoristsCoverageNS");
            if (flUMform != null)
            {
                foreach (var form in flUMform)
                {
                    form.IsSelected = false;
                    if (result.UMBILimitId >= 552 && result.UMBILimitId <= 566)
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update OR UM forms
            var orUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "OregonUninsuredMotoristCoverage");
            if (orUMform != null)
            {
                foreach (var form in orUMform)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 400 && result.UMBILimitId <= 408) || (result.UMPDLimitId == 520))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update OH UM forms
            var ohUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "OhioUMandUIMCoverage");
            if (ohUMform != null)
            {
                foreach (var form in ohUMform)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 389 && result.UMBILimitId <= 398) ||
                        (result.UMPDLimitId >= 510 && result.UMPDLimitId <= 519) ||
                        (result.UIMBILimitId >= 244 && result.UIMBILimitId <= 253))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update CO UM forms
            var coUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "ColoradoUMBICoverage");
            if (coUMform != null)
            {
                foreach (var form in coUMform)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 324 && result.UMBILimitId <= 332) ||
                        (result.UMBILimitId >= 632 && result.UMBILimitId <= 633) ||
                        (result.UMPDLimitId >= 446 && result.UMPDLimitId <= 447))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update CO MedPay forms
            var coMPform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "ColoradoMedicalPaymentCoverage");
            if (coMPform != null)
            {
                foreach (var form in coMPform)
                {
                    form.IsSelected = false;
                    if ((result.MedPayLimitId >= 111 && result.MedPayLimitId <= 116))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update IN UM forms
            var inUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "IndianaUninsuredMotoristCoverage");
            if (inUMform != null)
            {
                foreach (var form in inUMform)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 345 && result.UMBILimitId <= 353) ||
                        (result.UMBILimitId >= 647 && result.UMBILimitId <= 648) ||
                        (result.UMPDLimitId >= 474 && result.UMPDLimitId <= 487) ||
                        (result.UMPDLimitId >= 677 && result.UMPDLimitId <= 680))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update IN UIM forms
            var inUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "IndianaUnderinsuredMotoristCoverage");
            if (inUIMform != null)
            {
                foreach (var form in inUIMform)
                {
                    form.IsSelected = false;
                    if ((result.UIMBILimitId >= 202 && result.UIMBILimitId <= 209) ||
                        (result.UIMBILimitId >= 744 && result.UIMBILimitId <= 745))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update VA Split UM forms
            var vaSplitUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "VirginiaSplitUMCoverage");
            if (vaSplitUMform != null)
            {
                foreach (var form in vaSplitUMform)
                {
                    form.IsSelected = false;
                    if (_VASplitUMBILimits.Any(x => x == result.UMBILimitId))

                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update VA UM forms
            var vaUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "VirginiaImportantNoticeUMCoverage");
            if (vaUMform != null)
            {
                foreach (var form in vaUMform)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 665 && result.UMBILimitId <= 676) ||
                        (result.UMPDLimitId >= 540 && result.UMPDLimitId <= 550) ||
                        (result.UMPDLimitId >= 699 && result.UMPDLimitId <= 699))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update TN UM forms
            var tnUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "TennesseeUninsuredMotoristCoverage");
            if (tnUMform != null)
            {
                var tnLimits = (await _lvLimitsRepository.GetAsync(x => x.StateCode == "TN" && x.LimitDisplay != "NONE" &&
                                        (x.LimitType == "UMBI" || x.LimitType == "UMPD"))).ToList();
                foreach (var form in tnUMform)
                {
                    form.IsSelected = false;
                    if (tnLimits.Any(x => x.Id == result.UMBILimitId) || tnLimits.Any(x => x.Id == result.UMPDLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update IL UM forms
            var ilUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "IllinoisUMCoverage");
            if (ilUMform != null)
            {
                var ilLimits = (await _lvLimitsRepository.GetAsync(x => x.StateCode == "IL" && x.LimitDisplay != "NONE" &&
                                        (x.LimitType == "UMBI" || x.LimitType == "UMPD"))).ToList();
                foreach (var form in ilUMform)
                {
                    form.IsSelected = false;
                    if (ilLimits.Any(x => x.Id == result.UMBILimitId) || ilLimits.Any(x => x.Id == result.UMPDLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update IL UMPD forms
            var ilUMPDform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "IllinoisUMPDCoverage");
            if (ilUMPDform != null)
            {
                var ilLimits = (await _lvLimitsRepository.GetAsync(x => x.StateCode == "IL" && x.LimitDisplay != "NONE" &&
                                        (x.LimitType == "UMBI" || x.LimitType == "UMPD"))).ToList();
                foreach (var form in ilUMPDform)
                {
                    form.IsSelected = false;
                    if (ilLimits.Any(x => x.Id == result.UMBILimitId) || ilLimits.Any(x => x.Id == result.UMPDLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update IL UIM forms
            var ilUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "IllinoisUIMCoverage");
            if (ilUIMform != null)
            {
                var ilLimits = (await _lvLimitsRepository.GetAsync(x => x.StateCode == "IL" && x.LimitDisplay != "NONE" &&
                                        (x.LimitType == "UIMBI" || x.LimitType == "UIMPD"))).ToList();
                foreach (var form in ilUIMform)
                {
                    form.IsSelected = false;
                    if (ilLimits.Any(x => x.Id == result.UIMBILimitId) || ilLimits.Any(x => x.Id == result.UIMPDLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update AZ UM forms
            var azUIMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "ArizonaUIMCoverage");
            if (azUIMform != null)
            {
                var azLimits = (await _lvLimitsRepository.GetAsync(x => x.StateCode == "AZ" && x.LimitDisplay != "NONE" &&
                                        (x.LimitType == "UIMBI" || x.LimitType == "UIMPD"))).ToList();
                foreach (var form in ilUIMform)
                {
                    form.IsSelected = false;
                    if (azLimits.Any(x => x.Id == result.UIMBILimitId) || azLimits.Any(x => x.Id == result.UIMPDLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update AZ UIM forms
            var azUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "ArizonaUMCoverage");
            if (azUMform != null)
            {
                var azLimits = (await _lvLimitsRepository.GetAsync(x => x.StateCode == "AZ" && x.LimitDisplay != "NONE" &&
                                        (x.LimitType == "UMBI" || x.LimitType == "UMPD"))).ToList();
                foreach (var form in ilUIMform)
                {
                    form.IsSelected = false;
                    if (azLimits.Any(x => x.Id == result.UMBILimitId) || azLimits.Any(x => x.Id == result.UMPDLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update MS UM forms
            var msUMform = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "MississippiUninsuredMotoristCoverageNS");
            if (msUMform != null)
            {
                var msLimits = (await _lvLimitsRepository.GetAsync(x => x.StateCode == "MS" && x.LimitDisplay != "NONE" &&
                                        (x.LimitType == "UMBI" || x.LimitType == "UMPD"))).ToList();
                foreach (var form in msUMform)
                {
                    form.IsSelected = false;
                    if (msLimits.Any(x => x.Id == result.UMBILimitId) || msLimits.Any(x => x.Id == result.UMPDLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }
            
            // Update SC Split UM forms
            var scSplitUMForm = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "SouthCarolinaSplitUMCoverage");
            if (scSplitUMForm != null)
            {
                foreach (var form in scSplitUMForm)
                {
                    form.IsSelected = false;
                    if (_SCSplitUMBILimits.Any(x => x == result.UMBILimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update SC Split UIM forms
            var scSplitUIMForm = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "SouthCarolinaSplitUIMCoverage");
            if (scSplitUIMForm != null)
            {
                foreach (var form in scSplitUIMForm)
                {
                    form.IsSelected = false;
                    if (_SCSplitUIMBILimits.Any(x => x == result.UIMBILimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update SC UM forms
            var scUMForm = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "SouthCarolinaUMCoverage");
            if (scUMForm != null)
            {
                foreach (var form in scUMForm)
                {
                    form.IsSelected = false;
                    if ((result.UMBILimitId >= 794 && result.UMBILimitId <= 821) ||
                        (result.UMPDLimitId >= 694 && result.UMPDLimitId <= 696))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update SC UIM forms
            var scUIMForm = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "SouthCarolinaUIMCoverage");
            if (scUIMForm != null)
            {
                foreach (var form in scUIMForm)
                {
                    form.IsSelected = false;
                    if ((result.UIMBILimitId >= 760 && result.UIMBILimitId <= 787) ||
                        (result.UIMPDLimitId >= 788 && result.UIMPDLimitId <= 790))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update Covered Auto Designation Symbol Form
            var coveredAutoDesignationSymbolForm = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "CoveredAutoDesignationSymbol");
            if (coveredAutoDesignationSymbolForm != null)
            {
                foreach (var form in coveredAutoDesignationSymbolForm)
                {
                    //if previous al symbol is not symbol 10 then isselected = true
                    //if form is selected and symbol is 10 then update the 'other' value
                    //if old symbol and new symbol is 10 and form is not selected
                    if ((oldAlSymbol != 14 && oldAlSymbol != 15 && (result.ALSymbolId == 14 || result.ALSymbolId == 15)) ||
                        (form.IsSelected == true && (result.ALSymbolId == 14 || result.ALSymbolId == 15)) ||
                        (form.IsSelected == false && (oldAlSymbol == 14 || oldAlSymbol == 15) && (result.ALSymbolId == 14 || result.ALSymbolId == 15)))
                    {
                        if (oldAlSymbol == result.ALSymbolId)
                            continue;

                        form.IsSelected = true;
                        form.Other = JsonConvert.SerializeObject(
                            new CoveredAutoDesigSymbolDTO { 
                                CoveredAutoText = symbolTenResponses?.FirstOrDefault()?.Id.ToString(), 
                                TextForEndorsement = null 
                            }
                            , JsonSerializerSettings);
                    }
                    else
                    {
                        form.IsSelected = false;
                        form.Other = null;
                    }

                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            // Update Pollution Coverage Form
            var pollutionCoverageForm = await _riskFormRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && x.FormId == "PollutionCoverage");
            if(pollutionCoverageForm != null)
            {
                var pocLimits = (await _lvLimitsRepository.GetAsync(x => x.LimitDisplay != "NONE" && x.LimitType == "POC")).ToList();

                foreach (var form in pollutionCoverageForm)
                {
                    form.IsSelected = false;
                    if (pocLimits.Any(x => x.Id == result.PollutionCovLimitId))
                        form.IsSelected = true;
                    _riskFormRepository.Update(form);
                }
                await _riskFormRepository.SaveChangesAsync();
            }

            return model;
        }

        public async Task<SaveRiskCoverageDTO> UpdateRatingFactorAsync(SaveRiskCoverageDTO model)
        {
            var data = await _riskCoverageRepository.FindAsync(x => x.RiskDetailId == model.RiskDetailId && x.OptionNumber == model.OptionNumber);

            data.LiabilityExperienceRatingFactor = model.LiabilityExperienceRatingFactor;
            data.LiabilityScheduleRatingFactor = model.LiabilityScheduleRatingFactor;

            var result = _riskCoverageRepository.Update(data);
            await _riskCoverageRepository.SaveChangesAsync();
            return model;
        }

        public async Task UpdateLimitDataAsync(SaveRiskCoverageDTO model)
        {
            var data = await _riskCoverageRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId);
            if (data != null)
            {
                foreach (var item in data)
                {
                    item.AutoBILimitId = model.AutoBILimitId;
                    item.AutoPDLimitId = model.AutoPDLimitId;
                    item.MedPayLimitId = model.MedPayLimitId;
                    item.PIPLimitId = model.PIPLimitId;
                    item.UMBILimitId = model.UMBILimitId;
                    item.UMPDLimitId = model.UMPDLimitId;
                    item.UIMBILimitId = model.UIMBILimitId;
                    item.UIMPDLimitId = model.UIMPDLimitId;

                    _riskCoverageRepository.Update(item);
                }
                await _riskCoverageRepository.SaveChangesAsync();
            }
        }
    }
}
