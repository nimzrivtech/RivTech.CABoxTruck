using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class AdditionalTermService : IAdditionalTermService
    {
        private readonly IAdditionalTermRepository _additionalTermRepository;
        private readonly IMapper _mapper;

        public AdditionalTermService(IAdditionalTermRepository additionalTermRepository, IMapper mapper)
        {
            _additionalTermRepository = additionalTermRepository;
            _mapper = mapper;
        }

        public async Task<List<AdditionalTermDTO>> GetAllAsync()
        {
            var result = await _additionalTermRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<AdditionalTermDTO>>(result);
        }

        public async Task<AdditionalTermDTO> GetAdditionalTermAsync(byte id)
        {
            var result = await _additionalTermRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<AdditionalTermDTO>(result);
        }

        public async Task<string> InsertAdditionalTermAsync(AdditionalTermDTO model)
        {
            var result = await _additionalTermRepository.AddAsync(_mapper.Map<Data.Entity.LvAdditionalTerm>(model));
            await _additionalTermRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveAdditionalTermAsync(byte id)
        {
            var entity = await _additionalTermRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _additionalTermRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateAdditionalTermAsync(AdditionalTermDTO model)
        {
            var result = _additionalTermRepository.Update(_mapper.Map<Data.Entity.LvAdditionalTerm>(model));
            await _additionalTermRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
