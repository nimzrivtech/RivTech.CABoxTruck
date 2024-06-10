using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics
{
    public class RiskSpecificCommoditiesHauledService : IRiskSpecificCommoditiesHauledService
    {
        private readonly IMapper _mapper;
        private readonly IRiskSpecificCommoditiesHauledRepository _riskSpecificCommoditiesHauledRepository;

        public RiskSpecificCommoditiesHauledService(
            IRiskSpecificCommoditiesHauledRepository riskSpecificCommoditiesHauledRepository, IMapper mapper)
        {
            _riskSpecificCommoditiesHauledRepository = riskSpecificCommoditiesHauledRepository;
            _mapper = mapper;
        }

        public async Task<RiskSpecificCommoditiesHauledDto> InsertAsync(RiskSpecificCommoditiesHauledDto model)
        {
            var modelToSave = _mapper.Map<RiskSpecificCommoditiesHauled>(model);
            modelToSave.CreatedDate = DateTime.Now;
            var result = await _riskSpecificCommoditiesHauledRepository.AddAsync(modelToSave);
            await _riskSpecificCommoditiesHauledRepository.SaveChangesAsync();
            return _mapper.Map<RiskSpecificCommoditiesHauledDto>(result);
        }

        public async Task<List<RiskSpecificCommoditiesHauledDto>> GetByRiskDetailIdAsync(Guid riskId)
        {
            var result = await _riskSpecificCommoditiesHauledRepository.GetAsync(x => x.IsActive && x.RiskDetailId == riskId);
            return _mapper.Map<List<RiskSpecificCommoditiesHauledDto>>(result);
        }

        public async Task<bool> UpdateAsync(RiskSpecificCommoditiesHauledDto model)
        {
            var existingCommodity = await _riskSpecificCommoditiesHauledRepository.FindAsync(x => x.Id == model.Id);
            var modelToSave = _mapper.Map<RiskSpecificCommoditiesHauled>(existingCommodity);
            modelToSave.Commodity = model.Commodity;
            modelToSave.PercentageOfCarry = model.PercentageOfCarry;
            var result = _riskSpecificCommoditiesHauledRepository.Update(modelToSave);
            await _riskSpecificCommoditiesHauledRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var entity = await _riskSpecificCommoditiesHauledRepository.FindAsync(x => x.Id == id);

            _riskSpecificCommoditiesHauledRepository.Remove(entity);

            await _riskSpecificCommoditiesHauledRepository.SaveChangesAsync();
            return true;
        }
    }
}