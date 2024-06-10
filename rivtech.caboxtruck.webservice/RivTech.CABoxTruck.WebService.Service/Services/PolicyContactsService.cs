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
    public class PolicyContactsService : IPolicyContactsService
    {
        private readonly IPolicyContactsRepository _policyContactsRepository;
        private readonly IMapper _mapper;

        public PolicyContactsService(IPolicyContactsRepository policyContactsRepository, IMapper mapper)
        {
            _policyContactsRepository = policyContactsRepository;
            _mapper = mapper;
        }

        public async Task<List<PolicyContactsDTO>> GetAllPolicyContacts()
        {
            var result = await _policyContactsRepository.GetAllAsync();
            return _mapper.Map<List<PolicyContactsDTO>>(result);
        }

        public async Task<List<PolicyContactsDTO>> GetAllAssignedToPolicyContacts()
        {
            var result = await _policyContactsRepository.GetAsync(contact => contact.IsAssignedTo);
            return _mapper.Map<List<PolicyContactsDTO>>(result);
        }
    }
}
