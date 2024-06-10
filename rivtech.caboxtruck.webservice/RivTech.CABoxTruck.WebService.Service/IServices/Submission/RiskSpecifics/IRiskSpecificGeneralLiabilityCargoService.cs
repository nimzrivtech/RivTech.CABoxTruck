using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.GeneralLiabilityCargo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificGeneralLiabilityCargoService
    {
        Task<List<RiskResponseDTO>> Save(RiskSpecificGeneralLiabilityCargoDTO riskSpecificGeneralLiabilityCargo);
        Task<RiskSpecificGeneralLiabilityCargoDTO> GetGeneralLiabilityCargo(Guid riskId);
        Task<bool> Delete(Guid riskId, List<int> sectionIds);
    }
}
