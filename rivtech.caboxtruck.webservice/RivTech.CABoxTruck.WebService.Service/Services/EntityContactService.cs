using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class EntityContactService : IEntityContactService
    {
        private readonly IEntityContactRepository _entityContactRepository;
        private readonly IMapper _mapper;

        public EntityContactService(IEntityContactRepository entityContactRepository, IMapper mapper)
        {
            _entityContactRepository = entityContactRepository;
            _mapper = mapper;
        }

        public async Task<List<EntityContactDTO>> GetAllAsync()
        {
            var result = await _entityContactRepository.GetAllAsync();
            return _mapper.Map<List<EntityContactDTO>>(result);
        }

        public async Task<List<EntityContactDTO>> GetAllAsync(Guid id)
        {
            var result = await _entityContactRepository.GetAsync(x => x.EntityId == id);
            return _mapper.Map<List<EntityContactDTO>>(result);
        }

        public async Task<EntityContactDTO> GetEntityAsync(Guid id)
        {
            var result = await _entityContactRepository.FindAsync(x => x.Id == id);
            return _mapper.Map<EntityContactDTO>(result);
        }

        public async Task<string> InsertEntityAsync(EntityContactDTO model)
        {
            var result = await _entityContactRepository.AddAsync(_mapper.Map<Data.Entity.EntityContact>(model));
            await _entityContactRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<bool> InsertEntityBulkAsync(List<EntityContactDTO> contacts)
        {
            foreach(var contact in contacts)
            {
                await _entityContactRepository.AddAsync(_mapper.Map<Data.Entity.EntityContact>(contact));
            }
            await _entityContactRepository.SaveChangesAsync();
            return true;
        }

        public async Task<string> RemoveEntityAsync(Guid id)
        {
            var entity = await _entityContactRepository.FindAsync(x => x.Id == id);
            await _entityContactRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateEntityAsync(EntityContactDTO model)
        {
            var result = _entityContactRepository.Update(_mapper.Map<Data.Entity.EntityContact>(model));
            await _entityContactRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
