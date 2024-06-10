using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class InsuredService : IInsuredService
    {
        private readonly IInsuredRepository _insuredRepository;
        private readonly IMapper _mapper;

        public InsuredService(IInsuredRepository insuredRepository, IMapper mapper)
        {
            _insuredRepository = insuredRepository;
            _mapper = mapper;
        }

        public async Task<List<InsuredDTO>> GetAllAsync()
        {
            var result = await _insuredRepository.GetAllAsync();
            return _mapper.Map<List<InsuredDTO>>(result);
        }

        public async Task<InsuredDTO> GetInsuredAsync(Guid id)
        {
            var result = await _insuredRepository.FindAsync(x => x.Id == id);
            return _mapper.Map<InsuredDTO>(result);
        }

        public async Task<string> InsertInsuredAsync(InsuredDTO model)
        {
            var result = await _insuredRepository.AddAsync(_mapper.Map<Data.Entity.Insured>(model));
            await _insuredRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveInsuredAsync(Guid id)
        {
            var entity = await _insuredRepository.FindAsync(x => x.Id == id);
            var result = _insuredRepository.Remove(entity);
            await _insuredRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> UpdateInsuredAsync(InsuredDTO model)
        {
            var result = _insuredRepository.Update(_mapper.Map<Data.Entity.Insured>(model));
            await _insuredRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
