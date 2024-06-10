using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskFilingService : IRiskFilingService
    {
        private readonly IMapper _mapper;
        private readonly IRiskFilingRepository _riskFilingRepository;

        public RiskFilingService(IRiskFilingRepository riskFilingRepository, IMapper mapper)
        {
            _riskFilingRepository = riskFilingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RiskFilingDTO>> GetByRiskDetailIdAsync(Guid riskId)
        {
            var result = await _riskFilingRepository.GetAsync(rf => rf.RiskDetailId == riskId);
            return _mapper.Map<IEnumerable<RiskFilingDTO>>(result);
        }

        public async Task<int> InsertRangeAsync(IEnumerable<RiskFilingDTO> model)
        {
            var riskFilings = _mapper.Map<List<RiskFiling>>(model);

            await _riskFilingRepository.AddRangeAsync(riskFilings);
            var result = await _riskFilingRepository.SaveChangesAsync();

            return result;
        }

        public async Task<int> RemoveByRiskDetailIdAsync(Guid riskId)
        {
            var riskFilings = await _riskFilingRepository.GetAsync(rf => rf.RiskDetailId == riskId);
            _riskFilingRepository.RemoveRange(riskFilings);
            return await _riskFilingRepository.SaveChangesAsync();
        }
    }
}