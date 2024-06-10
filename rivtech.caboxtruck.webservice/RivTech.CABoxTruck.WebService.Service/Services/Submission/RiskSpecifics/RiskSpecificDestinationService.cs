using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics
{
    public class RiskSpecificDestinationService : IRiskSpecificDestinationService
    {
        private readonly IRiskSpecificDestinationRepository _riskSpecificDestinationRepository;
        private readonly IMapper _mapper;

        public RiskSpecificDestinationService(IRiskSpecificDestinationRepository riskSpecificDestinationRepository, IMapper mapper)
        {
            _riskSpecificDestinationRepository = riskSpecificDestinationRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskSpecificDestinationDto>> GetAllAsync(Guid riskId)
        {
            var result = await _riskSpecificDestinationRepository.GetAsync(x => x.IsActive && x.RiskDetailId == riskId);
            return _mapper.Map<List<RiskSpecificDestinationDto>>(result);
        }

        public async Task<RiskSpecificDestinationDto> GetDestinationAsync(Guid id)
        {
            var result = await _riskSpecificDestinationRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RiskSpecificDestinationDto>(result);
        }

        public async Task<string> InsertDestinationAsync(RiskSpecificDestinationDto model)
        {
            var modelToSave = _mapper.Map<RiskSpecificDestination>(model);
            modelToSave.IsActive = true;
            var result = await _riskSpecificDestinationRepository.AddAsync(modelToSave);
            await _riskSpecificDestinationRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveDestinationAsync(Guid id)
        {
            var entity = await _riskSpecificDestinationRepository.FindAsync(x => x.Id == id);
            if (entity != null)
            {
                _riskSpecificDestinationRepository.Remove(entity);
            }
            else
            {
                throw new ModelNotFoundException("Destination does not exist");
            }
            await _riskSpecificDestinationRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateDestinationAsync(RiskSpecificDestinationDto model)
        {
            var modelToSave = _mapper.Map<RiskSpecificDestination>(model);
            modelToSave.IsActive = true;
            var result = _riskSpecificDestinationRepository.Update(modelToSave);
            await _riskSpecificDestinationRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
