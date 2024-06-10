using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskPolicyContactService : IRiskPolicyContactService
    {
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IMapper _mapper;
        private readonly IRiskPolicyContactRepository _riskPolicyContactRepository;

        public RiskPolicyContactService(
            IRiskPolicyContactRepository riskPolicyContactRepository,
            IEntityAddressRepository entityAddressRepository,
            IEntityRepository entityRepository,
            IAddressRepository addressRepository,
            IRiskDetailRepository riskDetailRepository,
            IMapper mapper)
        {
            _riskPolicyContactRepository = riskPolicyContactRepository;
            _entityAddressRepository = entityAddressRepository;
            _entityRepository = entityRepository;
            _addressRepository = addressRepository;
            _riskDetailRepository = riskDetailRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RiskPolicyContactDTO>> GetByRiskDetailIdAsync(Guid riskDetailId)
        {
            var policyContacts = await _riskPolicyContactRepository.GetAllIncludeAsync(riskDetailId);
            var policyContactsDTOs = new List<RiskPolicyContactDTO>();

            foreach (var ai in policyContacts)
            {
                var entityAddress = ai.Entity.EntityAddresses.FirstOrDefault();

                var policyContactsDTO = new RiskPolicyContactDTO();
                policyContactsDTO.Id = ai.Id;
                policyContactsDTO.TypeId = ai.TypeId;
                policyContactsDTO.RiskDetailId = ai.RiskDetailId;
                policyContactsDTO.FirstName = ai.Entity.FirstName;
                policyContactsDTO.LastName = ai.Entity.LastName;
                policyContactsDTO.FullName = ai.Entity.FullName;
                policyContactsDTO.Email = ai.Entity.WorkEmailAddress;
                policyContactsDTO.Phone = ai.Entity.WorkPhone;
                policyContactsDTO.Address = entityAddress?.Address.StreetAddress1;
                policyContactsDTO.City = entityAddress?.Address.City;
                policyContactsDTO.State = entityAddress?.Address.State;
                policyContactsDTO.ZipCode = entityAddress?.Address.ZipCode;
                policyContactsDTO.CreatedBy = ai.CreatedBy;
                policyContactsDTO.CreatedDate = ai.CreatedDate;
                policyContactsDTO.UpdatedDate = ai.UpdatedDate;
                policyContactsDTO.DeletedDate = ai.DeletedDate;
                policyContactsDTO.IsActive = ai.DeletedDate == null ? true : false;

                policyContactsDTOs.Add(policyContactsDTO);
            }

            return policyContactsDTOs;
        }

        public async Task<RiskPolicyContactDTO> GetAsync(Guid id)
        {
            var result = await _riskPolicyContactRepository.GetIncludeAsync(id);
            if (result == null)
                return null;
            var entityAddress = result.Entity.EntityAddresses.FirstOrDefault();
            return new RiskPolicyContactDTO
            {
                Id = result.Id,
                TypeId = result.TypeId,
                RiskDetailId = result.RiskDetailId,
                FirstName = result.Entity.FirstName,
                LastName = result.Entity.LastName,
                //FullName = result.Entity.FullName,
                Email = result.Entity.WorkEmailAddress,
                Phone = result.Entity.WorkPhone,
                Address = entityAddress?.Address.StreetAddress1,
                City = entityAddress?.Address.City,
                State = entityAddress?.Address.State,
                ZipCode = entityAddress?.Address.ZipCode,
                CreatedBy = result.CreatedBy,
                CreatedDate = result.CreatedDate,
                UpdatedDate = result.UpdatedDate,
                DeletedDate = result.DeletedDate,
                IsActive = result.DeletedDate == null ? true : false
            };
        }

        public async Task<Guid> InsertAsync(RiskPolicyContactDTO model)
        {
            var policyContact = new RiskPolicyContact();
            policyContact.Id = Guid.NewGuid();
            policyContact.RiskDetailId = model.RiskDetailId;
            policyContact.TypeId = model.TypeId;
            policyContact.Entity = new Entity {
                FirstName = model.FirstName,
                LastName = model.LastName,
                //FullName = $"{model.FirstName} {model.LastName}",
                WorkEmailAddress = model.Email,
                WorkPhone = model.Phone,
                EntityAddresses = new List<EntityAddress>()
            };
            policyContact.Entity.EntityAddresses.Add(new EntityAddress()
            {
                Id = Guid.NewGuid(),
                Address = new Address()
                {
                    StreetAddress1 = model.Address,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode
                }
            });
            var result = await _riskPolicyContactRepository.AddAsync(policyContact);
            await _riskPolicyContactRepository.SaveChangesAsync();
            return result.Id;
        }
        public async Task<Guid> UpdateAsync(RiskPolicyContactDTO model)
        {
            var data = await _riskPolicyContactRepository.GetIncludeAsync(model.Id.Value);
            if (data == null)
                throw new Exception("Record doesn't exist");
            data.TypeId = model.TypeId;
            data.Entity.FirstName = model.FirstName;
            data.Entity.LastName = model.LastName;
            //data.Entity.FullName = $"{model.FirstName} {model.LastName}";
            data.Entity.WorkEmailAddress = model.Email;
            data.Entity.WorkPhone = model.Phone;
            foreach (var entityAddress in data.Entity.EntityAddresses)
            {
                entityAddress.Address.StreetAddress1 = model.Address;
                entityAddress.Address.City = model.City;
                entityAddress.Address.State = model.State;
                entityAddress.Address.ZipCode = model.ZipCode;
            }
            data.UpdatedDate = DateTime.Now;
            _riskPolicyContactRepository.Update(data);
            await _riskPolicyContactRepository.SaveChangesAsync();
            return data.Id;
        }

        public async Task<bool> RemoveAsync(Guid id, bool fromEndorsement)
        {
            var data = await _riskPolicyContactRepository.FindAsync(x => x.Id == id);

            if (!fromEndorsement)
            {
                _riskPolicyContactRepository.Remove(data);
                await _riskPolicyContactRepository.SaveChangesAsync();
                return true;
            }

            data.DeletedDate = DateTime.Now;
            _riskPolicyContactRepository.Update(data);
            await _riskPolicyContactRepository.SaveChangesAsync();
            return false;
        }
    }
}