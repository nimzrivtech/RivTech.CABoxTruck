using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class EntitySubsidiaryService : IEntitySubsidiaryService
    {
        private readonly IEntitySubsidiaryRepository _entitySubsidiaryRepository;
        private readonly IRiskDetailRepository _riskRepository;
        private readonly IMapper _mapper;

        public EntitySubsidiaryService(IEntitySubsidiaryRepository entitySubsidiaryRepository,
            IRiskDetailRepository riskRepository,
            IMapper mapper)
        {
            _entitySubsidiaryRepository = entitySubsidiaryRepository;
            _mapper = mapper;
            _riskRepository = riskRepository;
        }

        public async Task<EntitySubsidiaryDTO> GetByEntityIdAsync(Guid id)
        {
            var result = await _entitySubsidiaryRepository.FindAsync(x => x.Id == id);
            return _mapper.Map<EntitySubsidiaryDTO>(result);
        }

        public async Task<EntitySubsidiaryDTO> InsertorUpdateSubsidiaryAsync(SaveEntitySubsidiaryDTO model)
        {
            EntitySubsidiary result = null;
            var risk = await _riskRepository.GetInsuredAsync(model.RiskDetailId.Value);
            if(!model.Id.HasValue)
            {
                result = await _entitySubsidiaryRepository.AddAsync(new EntitySubsidiary() {
                    EntityId = risk.Insured.EntityId,
                    IsSubsidiaryCompany = true,
                    Name = model.Name,
                    TypeOfBusinessId = model.TypeOfBusinessId,
                    NumVechSizeComp = model.NumVechSizeComp,
                    Relationship = model.Relationship
                });
            }
            else
            {
                var entity = await _entitySubsidiaryRepository.FindAsync(x => x.Id == model.Id);
                entity.Name = model.Name;
                entity.NumVechSizeComp = model.NumVechSizeComp;
                entity.Relationship = model.Relationship;
                entity.TypeOfBusinessId = model.TypeOfBusinessId;
                result = _entitySubsidiaryRepository.Update(entity);
            }

            await _entitySubsidiaryRepository.SaveChangesAsync();
            return _mapper.Map<EntitySubsidiaryDTO>(result);
        }

        public async Task<bool> RemoveSubsidiaryAllAsync(Guid id)
        {
            var risk = await _riskRepository.GetInsuredAsync(id);
            var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == risk.Insured.EntityId && x.IsSubsidiaryCompany == true);

            _entitySubsidiaryRepository.RemoveRange(subsidiaries);
            await _entitySubsidiaryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<EntitySubsidiaryDTO> InsertorUpdateCompanyAsync(SaveEntitySubsidiaryDTO model)
        {
            EntitySubsidiary result = null;
            var risk = await _riskRepository.GetInsuredAsync(model.RiskDetailId.Value);
            if (!model.Id.HasValue)
            {
                result = await _entitySubsidiaryRepository.AddAsync(new EntitySubsidiary()
                {
                    EntityId = risk.Insured.EntityId,
                    IsCurrentlyOwned = true,
                    Name = model.Name,
                    NameYearOperations = model.NameYearOperations
                });
            }
            else
            {
                var entity = await _entitySubsidiaryRepository.FindAsync(x => x.Id == model.Id);
                entity.Name = model.Name;
                entity.NameYearOperations = model.NameYearOperations;
                result = _entitySubsidiaryRepository.Update(entity);
            }

            await _entitySubsidiaryRepository.SaveChangesAsync();
            return _mapper.Map<EntitySubsidiaryDTO>(result);
        }

        public async Task<bool> RemoveCompanyAllAsync(Guid id)
        {
            var risk = await _riskRepository.GetInsuredAsync(id);
            var companies = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == risk.Insured.EntityId && x.IsCurrentlyOwned == true);

            _entitySubsidiaryRepository.RemoveRange(companies);
            await _entitySubsidiaryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var entity = await _entitySubsidiaryRepository.FindAsync(x => x.Id == id);
            _entitySubsidiaryRepository.Remove(entity);
            await _entitySubsidiaryRepository.SaveChangesAsync();
            return true;
        }
    }
}
