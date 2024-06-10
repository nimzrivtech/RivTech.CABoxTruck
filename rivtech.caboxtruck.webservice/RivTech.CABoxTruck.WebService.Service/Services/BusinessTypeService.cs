using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class BusinessTypeService : IBusinessTypeService
    {
        private readonly IBusinessTypeRepository _businessTypeRepository;
        private readonly IMapper _mapper;
        public BusinessTypeService(IBusinessTypeRepository businessTypeRepository, IMapper mapper)
        {
            _businessTypeRepository = businessTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<BusinessTypeDTO>> GetAllAsync()
        {
            var result = await _businessTypeRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<BusinessTypeDTO>>(result);
        }

        public async Task<BusinessTypeDTO> GetBusinessTypeAsync(short id)
        {
            var result = await _businessTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<BusinessTypeDTO>(result);
        }

        public async Task<string> InsertBusinessTypeAsync(BusinessTypeDTO model)
        {
            var result = await _businessTypeRepository.AddAsync(_mapper.Map<BusinessType>(model));
            await _businessTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveBusinessTypeAsync(short id)
        {
            var entity = await _businessTypeRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _businessTypeRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateBusinessTypeAsync(BusinessTypeDTO model)
        {
            var result = _businessTypeRepository.Update(_mapper.Map<BusinessType>(model));
            await _businessTypeRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
