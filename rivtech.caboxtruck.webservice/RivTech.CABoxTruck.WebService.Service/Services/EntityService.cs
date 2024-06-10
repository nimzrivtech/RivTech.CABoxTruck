using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class EntityService : IEntityService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IMapper _mapper;

        public EntityService(IEntityRepository entityRepository, IMapper mapper)
        {
            _entityRepository = entityRepository;
            _mapper = mapper;
        }

        public async Task<List<EntityDTO>> GetAllAsync()
        {
            var result = await _entityRepository.GetAllAsync();
            return _mapper.Map<List<EntityDTO>>(result);
        }

        public async Task<EntityDTO> GetEntityAsync(Guid id)
        {
            var result = await _entityRepository.FindAsync(x => x.Id == id);
            return _mapper.Map<EntityDTO>(result);
        }

        public async Task<string> InsertEntityAsync(EntityDTO model)
        {
            var result = await _entityRepository.AddAsync(_mapper.Map<Data.Entity.Entity>(model));
            await _entityRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveEntityAsync(Guid id)
        {
            var entity = await _entityRepository.FindAsync(x => x.Id == id);
            await _entityRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateEntityAsync(EntityDTO model)
        {
            var result = _entityRepository.Update(_mapper.Map<Data.Entity.Entity>(model));
            await _entityRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
