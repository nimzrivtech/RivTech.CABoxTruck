using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics
{
    public class RiskSpecificDriverInfoService : IRiskSpecificDriverInfoService
    {
        private readonly IRiskSpecificDriverInfoRepository _riskSpecificDriverInfoRepository;
        private readonly IMapper _mapper;

        public RiskSpecificDriverInfoService(IRiskSpecificDriverInfoRepository riskSpecificDriverInfoRepository, IMapper mapper)
        {
            _riskSpecificDriverInfoRepository = riskSpecificDriverInfoRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskSpecificDriverInfoDto>> GetAllAsync()
        {
            var result = await _riskSpecificDriverInfoRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<RiskSpecificDriverInfoDto>>(result);
        }

        public async Task<RiskSpecificDriverInfoDto> GetDriverInfoAsync(Guid id)
        {
            var result = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RiskSpecificDriverInfoDto>(result);
        }

        public async Task<string> InsertDriverInfoAsync(RiskSpecificDriverInfoDto model)
        {
            var result = await _riskSpecificDriverInfoRepository.AddAsync(_mapper.Map<RiskSpecificDriverInfo>(model));
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveDriverInfoAsync(Guid id)
        {
            var entity = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateDriverInfoAsync(RiskSpecificDriverInfoDto model)
        {
            var result = _riskSpecificDriverInfoRepository.Update(_mapper.Map<RiskSpecificDriverInfo>(model));
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
