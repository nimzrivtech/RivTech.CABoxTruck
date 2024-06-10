using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RetailerAgentService : IRetailerAgentService
    {
        
        private readonly IRetailerAgentRepository _retailerAgentRepository;
        private readonly IMapper _mapper;

        public RetailerAgentService(IMapper mapper, IRetailerAgentRepository retailerAgentRepository)
        {
            _mapper = mapper;
            _retailerAgentRepository = retailerAgentRepository;
        }

        public async Task<List<RetailerAgentDTO>> GetAll()
        {
            var result = await _retailerAgentRepository.GetAsync(r => r.Entity != null, r => r.Entity);
            return _mapper.Map<List<RetailerAgentDTO>>(result);
        }

        public async Task<List<RetailerAgentDTO>> GetByRetailerId(Guid id)
        {
            var result = await _retailerAgentRepository.GetAsync(r => r.RetailerId == id, r => r.Entity);
            return _mapper.Map<List<RetailerAgentDTO>>(result);
        }

        public async Task<RetailerAgentDTO> GetById(Guid agentId)
        {
            var result = await _retailerAgentRepository.GetAsync(r => r.Id == agentId, r => r.Entity);
            return _mapper.Map<RetailerAgentDTO>(result.FirstOrDefault());
        }

        public async Task<RetailerAgentDTO> InsertAsync(RetailerAgentDTO resource)
        {
            var model = _mapper.Map<RetailerAgent>(resource);

            var result = await _retailerAgentRepository.AddAsync(model);

            await _retailerAgentRepository.SaveChangesAsync();
            return _mapper.Map<RetailerAgentDTO>(result);
        }

        public async Task<RetailerAgentDTO> UpdateAsync(RetailerAgentDTO resource)
        {
            var model = await _retailerAgentRepository.FindAsync(r => r.Id == resource.Id);

            model.IsActive = resource.IsActive;
            //model.LinkedAgencies = _mapper.Map<List<RetailerAgency>>(resource.LinkedAgencies);
            model.Entity = _mapper.Map<Entity>(resource.Entity);

            var result = _retailerAgentRepository.Update(model);

            await _retailerAgentRepository.SaveChangesAsync();
            return _mapper.Map<RetailerAgentDTO>(result);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var entity = await _retailerAgentRepository.FindAsync(x => x.Id == id);
            entity.IsActive = false;
            _retailerAgentRepository.Remove(entity);
            await _retailerAgentRepository.SaveChangesAsync();
            return true;
        }

    }
}