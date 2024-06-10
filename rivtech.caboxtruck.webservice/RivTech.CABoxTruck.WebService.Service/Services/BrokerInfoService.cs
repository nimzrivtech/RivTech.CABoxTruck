using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class BrokerInfoService : IBrokerInfoService
    {
        private readonly IBrokerInfoRepository _repository;
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskAdditionalInterestRepository _riskAdditionalInterestRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskSequenceService _riskSequence;
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;

        public BrokerInfoService(IBrokerInfoRepository repository,
            IMapper mapper,
            IEntityAddressRepository entityAddressRepository,
            IRiskDetailRepository riskDetailRepository,
            IRiskRepository riskRepository,
            IRiskSequenceService riskSequence,
            IVehicleService vehicleService,
            IRiskAdditionalInterestRepository riskAdditionalInterestRepository,
            IRiskManuscriptsRepository riskManuscriptsRepository
        )
        {
            _repository = repository;
            _mapper = mapper;
            _entityAddressRepository = entityAddressRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskAdditionalInterestRepository = riskAdditionalInterestRepository;
            _riskRepository = riskRepository;
            _riskSequence = riskSequence;
            _vehicleService = vehicleService;
            _riskManuscriptsRepository = riskManuscriptsRepository;
        }

        public async Task<List<BrokerInfoDTO>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();

            return _mapper.Map<List<BrokerInfoDTO>>(result);
        }

        public async Task<BrokerInfoDTO> GetByRiskDetailIdAsync(Guid id)
        {
            var result = await _repository.FindAsync(x => x.RiskId == id);
            var risk = await _riskRepository.FindAsync(x => x.Id == id);

            var brokerInfoDTO = _mapper.Map<BrokerInfoDTO>(result);

            if (risk != null && brokerInfoDTO != null)
            {
                brokerInfoDTO.InternalUWUserId = risk.InternalUWUserId;
                brokerInfoDTO.ProductUWUserId = risk.ProductUWUserId;
                brokerInfoDTO.PsrUserId = risk.PsrUserId;
                brokerInfoDTO.AssistantUnderwriterId = risk.AssistantUnderwriterId;
                brokerInfoDTO.TeamLeadUserId = risk.TeamLeadUserId;
            }

            return brokerInfoDTO;
        }

        public async Task<BrokerInfoDTO> GetByRiskDetailIdAllIncludeAsync(Guid id)
        {
            var result = await _repository.GetByRiskIdAllIncludeAsync(id);
            var risk = await _riskRepository.FindAsync(x => x.Id == id);

            var brokerInfoDTO = _mapper.Map<BrokerInfoDTO>(result);

            if (risk != null && brokerInfoDTO != null)
            {
                brokerInfoDTO.InternalUWUserId = risk.InternalUWUserId;
                brokerInfoDTO.ProductUWUserId = risk.ProductUWUserId;
                brokerInfoDTO.PsrUserId = risk.PsrUserId;
                brokerInfoDTO.AssistantUnderwriterId = risk.AssistantUnderwriterId;
                brokerInfoDTO.TeamLeadUserId = risk.TeamLeadUserId;
            }

            return brokerInfoDTO;
        }

        public async Task<List<EntityAddressDTO>> GetAddressesAsync(Guid entityId)
        {
            var addresses = await _entityAddressRepository.GetAsync(x => x.EntityId == entityId && x.AddressTypeId != null, i => i.Address);
            return _mapper.Map<List<EntityAddressDTO>>(addresses);
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _repository.AnyAsync(x => x.RiskId == id);
        }

        public async Task<BrokerInfoDTO> GetByIdAsync(Guid id)
        {
            var result = await _repository.FindAsync(x => x.Id == id);
            var risk = await _riskRepository.FindAsync(x => x.Id == id);

            var brokerInfoDTO = _mapper.Map<BrokerInfoDTO>(result);

            if (risk != null && brokerInfoDTO != null)
            {
                brokerInfoDTO.InternalUWUserId = risk.InternalUWUserId;
                brokerInfoDTO.ProductUWUserId = risk.ProductUWUserId;
                brokerInfoDTO.PsrUserId = risk.PsrUserId;
                brokerInfoDTO.AssistantUnderwriterId = risk.AssistantUnderwriterId;
                brokerInfoDTO.TeamLeadUserId = risk.TeamLeadUserId;
            }

            return brokerInfoDTO;
        }

        public async Task<BrokerInfoDTO> InsertAsync(BrokerInfoDTO model)
        {
            var obj = _mapper.Map<BrokerInfo>(model);
            obj.Id = Guid.NewGuid();

            var result = await _repository.AddAsync(obj);
            await _repository.SaveChangesAsync();

            // update interests' effective and expiration date
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetail => riskDetail.RiskId == model.RiskId);
            await UpdateAIEffectiveExpirationDatesAsync(model, riskDetail);
            //update manuscript effective and expiration date
            await UpdateManuscriptEffectiveExpirationDatesAsync(model, riskDetail);
            var risk = await _riskRepository.FindAsync(x => x.Id == model.RiskId);

            risk.InternalUWUserId = model.InternalUWUserId;
            risk.ProductUWUserId = model.ProductUWUserId;
            risk.PsrUserId = model.PsrUserId;
            risk.AssistantUnderwriterId = model.AssistantUnderwriterId;
            risk.TeamLeadUserId = model.TeamLeadUserId;

            // Update Policy number
            if (risk.PolicyNumber != null) // && 
                                           //(risk.IsRenewal == false || risk.IsFromSN == true || risk.IsFromRenewal == true))
            {
                var riskDetailInsured = await _riskDetailRepository.GetInsuredAddressByRiskIdAsync(risk.Id);
                var state = riskDetailInsured.Insured.Entity.EntityAddresses.Find(x => x.AddressTypeId == (short)AddressTypesEnum.Business).Address.State;
                risk.PolicyNumber = _riskSequence.UpdatePolicyNumber(risk.PolicyNumber, obj.ExpirationDate.Value, obj.EffectiveDate.Value, state);
            }

            await _riskRepository.SaveChangesAsync();

            await _vehicleService.UpdateVehicleEffectiveDateExpirationDate(riskDetail.Id, obj.EffectiveDate.Value, obj.ExpirationDate.Value);
           
            var returnData = _mapper.Map<BrokerInfoDTO>(obj);
            returnData.PolicyNumber = risk.PolicyNumber;
            returnData.InternalUWUserId = model.InternalUWUserId;
            returnData.ProductUWUserId = model.ProductUWUserId;
            returnData.PsrUserId = model.PsrUserId;
            returnData.AssistantUnderwriterId = model.AssistantUnderwriterId;
            returnData.TeamLeadUserId = model.TeamLeadUserId;

            return returnData;
        }

        public async Task<BrokerInfoDTO> UpdateAsync(BrokerInfoDTO model)
        {
            if (model.ExpirationDate == null) // set expiration date as defaultif null
            {
                model.ExpirationDate = model.EffectiveDate.Value.AddYears(1);
            }
            var obj = _repository.Update(_mapper.Map<BrokerInfo>(model));
            await _repository.SaveChangesAsync();

            // update interests' effective and expiration date
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetail => riskDetail.RiskId == model.RiskId);
            await UpdateAIEffectiveExpirationDatesAsync(model, riskDetail);
            //update manuscript effective and expiration date
            await UpdateManuscriptEffectiveExpirationDatesAsync(model, riskDetail);
            // Update Policy number
            var risk = await _riskRepository.FindAsync(x => x.Id == model.RiskId);

            risk.InternalUWUserId = model.InternalUWUserId;
            risk.ProductUWUserId = model.ProductUWUserId;
            risk.PsrUserId = model.PsrUserId;
            risk.AssistantUnderwriterId = model.AssistantUnderwriterId;
            risk.TeamLeadUserId = model.TeamLeadUserId;

            if (risk.PolicyNumber != null) // && 
                //(risk.IsRenewal == false || risk.IsFromSN == true || risk.IsFromRenewal == true))
            {
                var riskDetailInsured = await _riskDetailRepository.GetInsuredAddressAsync(riskDetail.Id);
                var state = riskDetailInsured.Insured.Entity.EntityAddresses.Find(x => x.AddressTypeId == (short)AddressTypesEnum.Business).Address.State;
                risk.PolicyNumber = _riskSequence.UpdatePolicyNumber(risk.PolicyNumber, model.ExpirationDate.Value, model.EffectiveDate.Value, state);
            }

            await _riskRepository.SaveChangesAsync();

            await _vehicleService.UpdateVehicleEffectiveDateExpirationDate(riskDetail.Id, obj.EffectiveDate.Value, obj.ExpirationDate.Value);

            var returnData = _mapper.Map<BrokerInfoDTO>(obj);
            returnData.PolicyNumber = risk.PolicyNumber;
            returnData.InternalUWUserId = model.InternalUWUserId;
            returnData.ProductUWUserId = model.ProductUWUserId;
            returnData.PsrUserId = model.PsrUserId;
            returnData.AssistantUnderwriterId = model.AssistantUnderwriterId;
            returnData.TeamLeadUserId = model.TeamLeadUserId;

            return returnData;
        }

        public async Task UpdateAIEffectiveExpirationDatesAsync(BrokerInfoDTO model, RiskDetail riskDetail)
        {
            var additionalInterest = await _riskAdditionalInterestRepository.GetAsync(interest => interest.RiskDetailId == riskDetail.Id);

            foreach (var interest in additionalInterest)
            {
                interest.EffectiveDate = model.EffectiveDate;
                interest.ExpirationDate = model.ExpirationDate;
                _riskAdditionalInterestRepository.Update(interest);
            }
            await _riskAdditionalInterestRepository.SaveChangesAsync();
        }

        public async Task<string> RemoveAsync(Guid id)
        {
            var entity = await _repository.FindAsync(x => x.RiskId == id);
             _repository.Remove(entity);
            await _repository.SaveChangesAsync();
            return id.ToString();
        }

        public async Task UpdateManuscriptEffectiveExpirationDatesAsync(BrokerInfoDTO model, RiskDetail riskDetail)
        {
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetail.Id);

            foreach (var ms in manuscripts)
            {
                ms.EffectiveDate = model.EffectiveDate.Value;
                ms.ExpirationDate = model.ExpirationDate.Value;
                _riskManuscriptsRepository.Update(ms);
            }
            await _riskManuscriptsRepository.SaveChangesAsync();
        }
    }
}
