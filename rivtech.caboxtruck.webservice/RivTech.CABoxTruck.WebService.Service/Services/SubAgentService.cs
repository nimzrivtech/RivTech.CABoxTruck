using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class SubAgentService : ISubAgentService
    {
        private readonly ISubAgentRepository _repository;

        private readonly IMapper _mapper;

        public SubAgentService(ISubAgentRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AgentSubAgencyDTO>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return _mapper.Map<List<AgentSubAgencyDTO>>(result);
        }

        public async Task<List<AgentSubAgencyDTO>> GetByAgencyIdAsync(Guid id)
        {
            var result = await _repository.GetAsync(x => x.SubAgencyId == id && x.IsActive != false);
            return result.Select(obj => _mapper.Map<AgentSubAgencyDTO>(result)).ToList();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _repository.AnyAsync(x => x.Id == id);
        }

        public async Task<AgentSubAgencyDTO> GetByIdAsync(Guid id)
        {
            var result = await _repository.FindAsync(x => x.Id == id);
            return _mapper.Map<AgentSubAgencyDTO>(result);
        }

        public async Task<AgentSubAgencyDTO> InsertAsync(AgentSubAgencyDTO model)
        {
            var obj = _mapper.Map<SubAgent>(model);
            obj.Id = Guid.NewGuid();
            obj.IsActive = true;
            var result = await _repository.AddAsync(obj);
            await _repository.SaveChangesAsync();

            return _mapper.Map<AgentSubAgencyDTO>(result);
        }

        public async Task<AgentSubAgencyDTO> UpdateAsync(AgentSubAgencyDTO model)
        {
            var obj = _repository.Update(_mapper.Map<SubAgent>(model));
            await _repository.SaveChangesAsync();
            return _mapper.Map<AgentSubAgencyDTO>(obj);
        }

        public async Task<string> RemoveAsync(Guid id)
        {
            var entity = await _repository.FindAsync(x => x.Id == id);
            entity.IsActive = false;
            await _repository.SaveChangesAsync();
            return id.ToString();
        }
        
        public async Task<List<AgentSubAgencyDTO>> GetByAgencyIdIncludeAsync(Guid id)
        {
            var result = await _repository.GetAllIncludeAsync(x => x.SubAgencyId == id && x.IsActive != false);
            return _mapper.Map<List<AgentSubAgencyDTO>>(result).ToList();
        }

        public async Task<List<AgentSubAgencyDTO>> GetAllIncludeAsync()
        {
            var result = await _repository.GetAllIncludeAsync();
            return _mapper.Map<List<AgentSubAgencyDTO>>(result).ToList();

        }

        public async Task<AgentSubAgencyDTO> GetByIdIncludeAsync(Guid id)
        {
            var result = await _repository.GetAllIncludeAsync(o => o.Id == id);
            return _mapper.Map<AgentSubAgencyDTO>(result.FirstOrDefault() ?? null);
        }
    }
}
