using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.GeneralLiabilityCargo;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics
{
    public class RiskSpecificGeneralLiabilityCargoService : IRiskSpecificGeneralLiabilityCargoService
    {
        private readonly IRiskResponseService _riskResponseService;

        public RiskSpecificGeneralLiabilityCargoService(
            IRiskResponseService riskResponseService
        )
        {
            _riskResponseService = riskResponseService;
        }

        public async Task<List<RiskResponseDTO>> Save(RiskSpecificGeneralLiabilityCargoDTO riskSpecificGeneralLiabilityCargo)
        {
            var genLiability = await _riskResponseService.MapToRiskResponse(riskSpecificGeneralLiabilityCargo.GeneralLiability, riskSpecificGeneralLiabilityCargo.RiskDetailId);
            var cargo = await _riskResponseService.MapToRiskResponse(riskSpecificGeneralLiabilityCargo.Cargo, riskSpecificGeneralLiabilityCargo.RiskDetailId);
            
            var response = await _riskResponseService.SaveRiskResponse(genLiability, LvQuestionSection.GeneralLiability.Id, riskSpecificGeneralLiabilityCargo.RiskDetailId);
            response.AddRange(await _riskResponseService.SaveRiskResponse(cargo, LvQuestionSection.Cargo.Id, riskSpecificGeneralLiabilityCargo.RiskDetailId));
            return response; 
        }

        public async Task<RiskSpecificGeneralLiabilityCargoDTO> GetGeneralLiabilityCargo(Guid riskId)
        {
            var genLiabilityCargo = new RiskSpecificGeneralLiabilityCargoDTO();
            var response = await _riskResponseService.GetRiskResponseAllInclude(riskId, LvQuestionSection.GeneralLiability.Id);
            genLiabilityCargo.GeneralLiability = _riskResponseService.MapToEntityType(new RiskSpecificGeneralLiabilityDTO(), response);
            response = await _riskResponseService.GetRiskResponseAllInclude(riskId, LvQuestionSection.Cargo.Id);
            genLiabilityCargo.Cargo = _riskResponseService.MapToEntityType(new RiskSpecificCargoDTO(), response);
            genLiabilityCargo.RiskDetailId = riskId;

            return genLiabilityCargo;
        }

        public Task<bool> Delete(Guid riskId, List<int> sectionIds)
        {
            return _riskResponseService.Delete(riskId, sectionIds);
        }
    }
}
