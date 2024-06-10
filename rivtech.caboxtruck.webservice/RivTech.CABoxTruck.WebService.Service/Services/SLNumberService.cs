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
    public class SLNumberService : ISLNumberService
    {
        private readonly ISLNumberRepository _repository;

        private readonly IMapper _mapper;

        public SLNumberService(ISLNumberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SLNumberDTO>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return _mapper.Map<List<SLNumberDTO>>(result);
        }

        public async Task<SLNumberDTO> GetByIdAsync(Int16 id)
        {
            var result = await _repository.FindAsync(x => x.Id == id);
            return _mapper.Map<SLNumberDTO>(result);
        }

        public async Task<List<SLNumberDTO>> GetByCreditOfficeIdAsync(Int16? creditId)
        {
            if (creditId == null)return new List<SLNumberDTO>();
            var result = await _repository.GetAsync(x => x.CreditedOfficeId == creditId.Value);
            return _mapper.Map<List<SLNumberDTO>>(result.ToList());
        }

        public async Task<SLNumberDTO> GetByCreditOfficeIdStateAsync(Int16? creditId, string state)
        {
            if(creditId == null) return new SLNumberDTO();
            var result = await _repository.FindAsync(x => x.CreditedOfficeId == creditId.Value && x.StateCode.ToLower() == state.ToLower());
            return _mapper.Map<SLNumberDTO>(result);
        }

        public async Task<bool> IsExistAsync(Int16 id)
        {
            return await _repository.AnyAsync(x => x.Id == id);
        }

    }
}
