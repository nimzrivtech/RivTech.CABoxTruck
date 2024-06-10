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
    public class RelatedEntityTypeService : IRelatedEntityTypeService
    {
        private readonly IRelatedEntityTypeRepository _relatedEntityTypeRepository;
        private readonly IMapper _mapper;
        public RelatedEntityTypeService(IRelatedEntityTypeRepository relatedEntityTypeRepository, IMapper mapper)
        {
            _relatedEntityTypeRepository = relatedEntityTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<RelatedEntityTypeDTO>> GetAllAsync()
        {
            var result = await _relatedEntityTypeRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<RelatedEntityTypeDTO>>(result);
        }

        public async Task<RelatedEntityTypeDTO> GetRelatedEntityTypeAsync(byte id)
        {
            var result = await _relatedEntityTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RelatedEntityTypeDTO>(result);
        }

        public async Task<string> InsertRelatedEntityTypeAsync(RelatedEntityTypeDTO model)
        {
            var result = await _relatedEntityTypeRepository.AddAsync(_mapper.Map<Data.Entity.LvRelatedEntityType>(model));
            await _relatedEntityTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public Task<string> InsertRelatedTypeAsync(RelatedEntityTypeDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RemoveRelatedEntityTypeAsync(byte id)
        {
            var entity = await _relatedEntityTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _relatedEntityTypeRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateRelatedEntityTypeAsync(RelatedEntityTypeDTO model)
        {
            var result = _relatedEntityTypeRepository.Update(_mapper.Map<Data.Entity.LvRelatedEntityType>(model));
            await _relatedEntityTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
