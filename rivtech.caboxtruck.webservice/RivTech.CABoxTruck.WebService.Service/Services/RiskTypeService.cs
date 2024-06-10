using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskTypeService : IRiskTypeService
    {
        private readonly IRiskTypeRepository _riskTypeRepository;
        private readonly IMapper _mapper;
        public RiskTypeService(IRiskTypeRepository riskTypeRepository, IMapper mapper)
        {
            _riskTypeRepository = riskTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskTypeDTO>> GetAllAsync()
        {
            var result = await _riskTypeRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<RiskTypeDTO>>(result);
        }

        public async Task<RiskTypeDTO> GetRiskTypeAsync(byte id)
        {
            var result = await _riskTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RiskTypeDTO>(result);
        }

        public async Task<string> InsertRiskTypeAsync(RiskTypeDTO model)
        {
            var riskType = _mapper.Map<LvRiskType>(model);
            riskType.CreatedAt = DateTime.Now;
            var result = await _riskTypeRepository.AddAsync(riskType);
            await _riskTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveRiskTypeAsync(byte id)
        {
            var entity = await _riskTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _riskTypeRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateRiskTypeAsync(RiskTypeDTO model)
        {
            var result = _riskTypeRepository.Update(_mapper.Map<Data.Entity.LvRiskType>(model));
            await _riskTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
