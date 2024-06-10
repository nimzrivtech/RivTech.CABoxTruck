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
    public class RiskSpecificDriverHiringCriteriaService : IRiskSpecificDriverHiringCriteriaService
    {
        private readonly IRiskSpecificDriverHiringCriteriaRepository _riskSpecificDriverHiringCriteriaRepository;
        private readonly IMapper _mapper;

        public RiskSpecificDriverHiringCriteriaService(IRiskSpecificDriverHiringCriteriaRepository riskSpecificDriverHiringCriteriaRepository, IMapper mapper)
        {
            _riskSpecificDriverHiringCriteriaRepository = riskSpecificDriverHiringCriteriaRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskSpecificDriverHiringCriteriaDto>> GetAllAsync()
        {
            var result = await _riskSpecificDriverHiringCriteriaRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<RiskSpecificDriverHiringCriteriaDto>>(result);
        }

        public async Task<RiskSpecificDriverHiringCriteriaDto> GetDriverHiringCriteriaAsync(Guid id)
        {
            var result = await _riskSpecificDriverHiringCriteriaRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RiskSpecificDriverHiringCriteriaDto>(result);
        }

        public async Task<string> InsertDriverHiringCriteriaAsync(RiskSpecificDriverHiringCriteriaDto model)
        {
            var result = await _riskSpecificDriverHiringCriteriaRepository.AddAsync(_mapper.Map<RiskSpecificDriverHiringCriteria>(model));
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveDriverHiringCriteriaAsync(Guid id)
        {
            var entity = await _riskSpecificDriverHiringCriteriaRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateDriverHiringCriteriaAsync(RiskSpecificDriverHiringCriteriaDto model)
        {
            var result = _riskSpecificDriverHiringCriteriaRepository.Update(_mapper.Map<RiskSpecificDriverHiringCriteria>(model));
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
