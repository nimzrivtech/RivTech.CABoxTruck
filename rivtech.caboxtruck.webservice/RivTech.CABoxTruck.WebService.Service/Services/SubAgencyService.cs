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
    public class SubAgencyService : ISubAgencyService
    {
        private readonly ISubAgencyRepository _repository;
        private readonly IMapper _mapper;

        public SubAgencyService(ISubAgencyRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SubAgencyDTO>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return _mapper.Map<List<SubAgencyDTO>>(result);
        }

        public async Task<List<SubAgencyDTO>> GetByAgencyIdAsync(Guid id)
        {
            var result = await _repository.GetAsync(x => x.AgencyId == id);
            return result.Select(obj => _mapper.Map<SubAgencyDTO>(result)).ToList();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _repository.AnyAsync(x => x.Id == id);
        }

        public async Task<SubAgencyDTO> GetByIdAsync(Guid id)
        {
            var result = await _repository.FindAsync(x => x.Id == id);
            return _mapper.Map<SubAgencyDTO>(result);
        }

        public async Task<SubAgencyDTO> InsertAsync(SubAgencyDTO model)
        {
            var obj = _mapper.Map<SubAgency>(model);

            if (obj.Id == Guid.Empty) obj.Id = Guid.NewGuid();
            if (obj.Entity != null && obj.Entity.Id != Guid.Empty) obj.EntityId = obj.Entity.Id;
            obj.IsActive = true;
            var result = await _repository.AddAsync(obj);
            await _repository.SaveChangesAsync();

            return _mapper.Map<SubAgencyDTO>(result);
        }

        public async Task<SubAgencyDTO> UpdateAsync(SubAgencyDTO model)
        {
            var obj = _repository.Update(_mapper.Map<SubAgency>(model));
            await _repository.SaveChangesAsync();
            return _mapper.Map<SubAgencyDTO>(obj);
        }

        public async Task<string> RemoveAsync(Guid id)
        {
            var entity = await _repository.FindAsync(x => x.Id == id);
            entity.IsActive = false;
            await _repository.SaveChangesAsync();
            return id.ToString();
        }
        
        public async Task<List<SubAgencyDTO>> GetByAgencyIdIncludeAsync(Guid id)
        {
            var result = await _repository.GetAllIncludeAsync(x => x.AgencyId == id && x.IsActive != false);
            return _mapper.Map<List<SubAgencyDTO>>(result).ToList();
        }

        public async Task<List<SubAgencyDTO>> GetAllIncludeAsync()
        {
            var result = await _repository.GetAllIncludeAsync();
            return _mapper.Map<List<SubAgencyDTO>>(result).ToList();

        }

        public async Task<SubAgencyDTO> GetByIdIncludeAsync(Guid id)
        {
            var result = await _repository.GetAllIncludeAsync(o => o.Id == id);
            return _mapper.Map<SubAgencyDTO>(result.FirstOrDefault() ?? null);
        }
    }
}
