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
    public class RateTypeService : IRateTypeService
    {
        private readonly IRateTypeRepository _rateTypeRepository;
        private readonly IMapper _mapper;
        public RateTypeService(IRateTypeRepository rateTypeRepository, IMapper mapper)
        {
            _rateTypeRepository = rateTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<RateTypeDTO>> GetAllAsync()
        {
            var result = await _rateTypeRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<RateTypeDTO>>(result);
        }

        public async Task<RateTypeDTO> GetRateTypeAsync(byte id)
        {
            var result = await _rateTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RateTypeDTO>(result);
        }

        public async Task<string> InsertRateTypeAsync(RateTypeDTO model)
        {
            var result = await _rateTypeRepository.AddAsync(_mapper.Map<Data.Entity.LvRateType>(model));
            await _rateTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveRateTypeAsync(byte id)
        {
            var entity = await _rateTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _rateTypeRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateRateTypeAsync(RateTypeDTO model)
        {
            var result = _rateTypeRepository.Update(_mapper.Map<Data.Entity.LvRateType>(model));
            await _rateTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
