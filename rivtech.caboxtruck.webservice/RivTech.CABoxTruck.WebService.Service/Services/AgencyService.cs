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
    public class AgencyService : IAgencyService
    {
        private readonly IAgencyRepository _repository;
        private readonly IMapper _mapper;

        public AgencyService(IAgencyRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AgencyDTO>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return _mapper.Map<List<AgencyDTO>>(result);
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _repository.AnyAsync(x => x.Id == id);
        }

        public async Task<AgencyDTO> GetByIdAsync(Guid id)
        {
            var result = await _repository.FindAsync(x => x.Id == id);
            return _mapper.Map<AgencyDTO>(result);
        }

        public async Task<AgencyDTO> InsertAsync(AgencyDTO model)
        {
            var obj = _mapper.Map<Agency>(model);
            if (obj.Id == Guid.Empty) obj.Id = Guid.NewGuid();
            if (obj.Entity != null && obj.Entity.Id != Guid.Empty) obj.EntityId = obj.Entity.Id;

            obj.IsActive = true;

            var result = await _repository.AddAsync(obj);

            await _repository.SaveChangesAsync();

            return _mapper.Map<AgencyDTO>(result);
        }

        public async Task<long> InsertRangeAsync(List<AgencyDTO> model)
        {
            model.ForEach(o =>
            {
                if (o.Id == Guid.Empty) o.Id = Guid.NewGuid();
                if (o.Entity != null && o.Entity.Id != Guid.Empty) o.EntityId = o.Entity.Id;
                o.IsActive = true;
            });

            List<Agency> obj = _mapper.Map<List<Agency>>(model);

            await _repository.AddRangeAsync(obj.AsEnumerable());
            return await _repository.SaveChangesAsync();

        }

        public async Task<AgencyDTO> UpdateAsync(AgencyDTO model)
        {
            var vehicle = _repository.Update(_mapper.Map<Agency>(model));
            await _repository.SaveChangesAsync();
            return _mapper.Map<AgencyDTO>(vehicle);
        }

        public async Task<string> RemoveAsync(Guid id)
        {
            var entity = await _repository.FindAsync(x => x.Id == id);
            entity.IsActive = false;
            await _repository.SaveChangesAsync();
            return id.ToString();
        }

        public async Task<List<AgencyDTO>> GetAllIncludeAsync()
        {
            var result = await _repository.GetAllIncludeAsync();
            return _mapper.Map<List<AgencyDTO>>(result).ToList();
        }

        public async Task<AgencyDTO> GetByIdIncludeAsync(Guid id)
        {
            var result = await _repository.GetAllIncludeAsync(o => o.Id == id);
            return _mapper.Map<AgencyDTO>(result.FirstOrDefault() ?? null);
        }
    }
}
