using AutoMapper;
using RestSharp.Extensions;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class LimitsService : ILimitsService
    {
        private readonly ILvLimitsRepository _lvLimitsRepository;
        private readonly IMapper _mapper;
        public LimitsService(ILvLimitsRepository lvLimitsRepository,
            IMapper mapper)
        {
            _lvLimitsRepository = lvLimitsRepository;
            _mapper = mapper;
        }

        public async Task<LimitListDTO> GetPerStateAsync(string stateCode, int? rateVersionId = null)
        {
            LimitListDTO result = new LimitListDTO();

            result.UMBILimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.UMBI && x.StateCode == stateCode));
            result.UMPDLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.UMPD && x.StateCode == stateCode));
            result.UIMBILimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.UIMBI && x.StateCode == stateCode));
            result.UIMPDLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.UIMPD && x.StateCode == stateCode));
            result.PipLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.PIP && x.StateCode == stateCode));

            result.AutoBILimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.AutoBI));
            result.AutoPDLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.AutoPD));
            result.MedPayLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.MedPay));

            result.ComprehensiveLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.Comprehensive));
            result.CollisionLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.Collision));
            result.GlLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.GL));

            result.CargoLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.Cargo));
            result.RefLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.RefBreakdown));

            result.TrailerInterchangeLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.TrailerInterchange));
            result.NonOwnedTrailerLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.NonOwnedTrailer));
            result.TrailerTypeLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.TrailerType));
            result.PollutionCovLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.PollutionCov));

            var liaLimits = _mapper.Map<List<LimitsDTO>>(await _lvLimitsRepository.GetAsync(x => x.IsActive && x.LimitType == LimitType.LiabilityDeductible));
            if (rateVersionId.HasValue)
            {
                string workingRaterVersion = rateVersionId.ToString();
                if (rateVersionId <= 7)
                    result.LiabilityDeductibleLimits = liaLimits.Where(x => x.RaterVersionId?.Split(',').Contains(workingRaterVersion) == true).ToList();
                // Latest GL rater version = 7
                if (rateVersionId >= 7)
                {
                    result.GlLimits = result.GlLimits.Where(x => x.RaterVersionId?.Split(',').Contains("7") == true).ToList();
                }

                if (rateVersionId >= 8)
                    result.LiabilityDeductibleLimits = liaLimits.Where(x => x.RaterVersionId?.Split(',').Contains("8") == true).ToList();
            }
            else
            {
                // Update every rater version upgrade
                var latestRaterVersion = "8";
                result.LiabilityDeductibleLimits = liaLimits.Where(x => x.RaterVersionId?.Split(',').Contains(latestRaterVersion) == true).ToList();
            }

            return result;
        }
    }
}
