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
    public class ClaimsHistoryService : IClaimsHistoryService
    {
        private readonly IClaimsHistoryRepository _claimsHistoryRepository;
        private readonly IMapper _mapper;

        public ClaimsHistoryService(IClaimsHistoryRepository claimsHistoryRepository, IMapper mapper)
        {
            _claimsHistoryRepository = claimsHistoryRepository;
            _mapper = mapper;
        }

        public async Task<List<ClaimsHistoryDTO>> GetByRiskDetailIdAsync(Guid riskId)
        {
            var claimsHistory = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == riskId);
            return _mapper.Map<List<ClaimsHistoryDTO>>(claimsHistory);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _claimsHistoryRepository.AnyAsync(x => x.RiskDetailId == id);
        }

        public async Task<List<ClaimsHistoryDTO>> InsertAsync(List<ClaimsHistoryDTO> claimsHistories)
        {
            await _claimsHistoryRepository.AddRangeAsync(_mapper.Map<List<ClaimsHistory>>(claimsHistories));
            await _claimsHistoryRepository.SaveChangesAsync();
            return claimsHistories;
        }

        public async Task<List<ClaimsHistoryDTO>> UpdateAsync(List<ClaimsHistoryDTO> claimsHistories)
        {
            foreach (var item in claimsHistories)
            {
                var data = await _claimsHistoryRepository.FindAsync(x =>
                    x.RiskDetailId == item.RiskDetailId && x.Order == item.Order);
                if (data is null)
                    continue;

                _mapper.Map(item, data);
                _claimsHistoryRepository.Update(data);
            }

            await _claimsHistoryRepository.SaveChangesAsync();

            return claimsHistories;
        }
    }
}