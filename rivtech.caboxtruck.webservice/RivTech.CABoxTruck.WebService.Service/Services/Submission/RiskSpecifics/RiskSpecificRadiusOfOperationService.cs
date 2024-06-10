using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics
{
    public class RiskSpecificRadiusOfOperationService : IRiskSpecificRadiusOfOperationService
    {
        private readonly IRiskSpecificRadiusOfOperationRepository _riskSpecificRadiusOfOperationRepository;
        private readonly IRiskDetailRepository _riskRepository;
        private readonly IMapper _mapper;

        public RiskSpecificRadiusOfOperationService(IRiskSpecificRadiusOfOperationRepository riskSpecificRadiusOfOperationRepository,
            IRiskDetailRepository riskRepository, IMapper mapper)
        {
            _riskSpecificRadiusOfOperationRepository = riskSpecificRadiusOfOperationRepository;
            _riskRepository = riskRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskSpecificRadiusOfOperationDto>> GetAllAsync()
        {
            var result = await _riskSpecificRadiusOfOperationRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<RiskSpecificRadiusOfOperationDto>>(result);
        }

        public async Task<RiskSpecificRadiusOfOperationDto> GetRadiusOfOperationAsync(Guid id)
        {
            var result = await _riskSpecificRadiusOfOperationRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RiskSpecificRadiusOfOperationDto>(result);
        }

        public async Task<string> InsertRadiusOfOperationAsync(RiskSpecificRadiusOfOperationDto model)
        {
            var result = await _riskSpecificRadiusOfOperationRepository.AddAsync(_mapper.Map<RiskSpecificRadiusOfOperation>(model));
            await _riskSpecificRadiusOfOperationRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveRadiusOfOperationAsync(Guid id)
        {
            var entity = await _riskSpecificRadiusOfOperationRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _riskSpecificRadiusOfOperationRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateRadiusOfOperationAsync(Guid riskId, RiskSpecificRadiusOfOperationDto model)
        {
            var radiusOfOperationsFromDb = await _riskSpecificRadiusOfOperationRepository.FindAsync(riskId);
            if (radiusOfOperationsFromDb != null)
            {
                model.Id = radiusOfOperationsFromDb.Id;
                _mapper.Map<RiskSpecificRadiusOfOperationDto, RiskSpecificRadiusOfOperation>(model, radiusOfOperationsFromDb);
                radiusOfOperationsFromDb.IsActive = true;
                await _riskSpecificRadiusOfOperationRepository.SaveChangesAsync();
                return radiusOfOperationsFromDb.Id.ToString();
            }

            var risk = await _riskRepository.FindAsync(riskId);
            if (risk == null)
            {
                throw new ModelNotFoundException("Risk data not found");
            }

            risk.RadiusOfOperations = _mapper.Map<RiskSpecificRadiusOfOperation>(model);
            risk.RadiusOfOperations.IsActive = true;

            await _riskRepository.SaveChangesAsync();
            return riskId.ToString();
        }
    }
}
