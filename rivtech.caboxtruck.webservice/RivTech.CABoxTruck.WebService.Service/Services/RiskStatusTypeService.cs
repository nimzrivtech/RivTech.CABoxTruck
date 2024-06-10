using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskStatusTypeService : IRiskStatusTypeService
    {
        private readonly IRiskStatusTypeRepository _riskStatusTypeRepository;
        private readonly IMapper _mapper;
        public RiskStatusTypeService(IRiskStatusTypeRepository riskStatusTypeRepository, IMapper mapper)
        {
            _riskStatusTypeRepository = riskStatusTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskStatusTypeDTO>> GetAllAsync()
        {
            var result = await _riskStatusTypeRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<RiskStatusTypeDTO>>(result);
        }

        public async Task<RiskStatusTypeDTO> GetRiskStatusTypeAsync(byte id)
        {
            var result = await _riskStatusTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RiskStatusTypeDTO>(result);
        }

        public async Task<string> InsertRiskStatusTypeAsync(RiskStatusTypeDTO model)
        {
            var result = await _riskStatusTypeRepository.AddAsync(_mapper.Map<Data.Entity.LvRiskStatusType>(model));
            await _riskStatusTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveRiskStatusTypeAsync(byte id)
        {
            var entity = await _riskStatusTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _riskStatusTypeRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateRiskStatusTypeAsync(RiskStatusTypeDTO model)
        {
            var result = _riskStatusTypeRepository.Update(_mapper.Map<Data.Entity.LvRiskStatusType>(model));
            await _riskStatusTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
