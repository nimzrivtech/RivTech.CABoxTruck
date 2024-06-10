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
    public class RiskSpecificUnderwritingQuestionService : IRiskSpecificUnderwritingQuestionService
    {
        private readonly IRiskSpecificUnderwritingQuestionRepository _riskSpecificUnderwritingQuestionRepository;
        private readonly IRiskDetailRepository _riskRepository;
        private readonly IMapper _mapper;

        public RiskSpecificUnderwritingQuestionService(IRiskSpecificUnderwritingQuestionRepository riskSpecificUnderwritingQuestionRepository,
            IRiskDetailRepository riskRepository, IMapper mapper)
        {
            _riskSpecificUnderwritingQuestionRepository = riskSpecificUnderwritingQuestionRepository;
            _riskRepository = riskRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskSpecificUnderwritingQuestionDto>> GetAllAsync()
        {
            var result = await _riskSpecificUnderwritingQuestionRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<RiskSpecificUnderwritingQuestionDto>>(result);
        }

        public async Task<RiskSpecificUnderwritingQuestionDto> GetUnderwritingQuestionAsync(Guid id)
        {
            var result = await _riskSpecificUnderwritingQuestionRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<RiskSpecificUnderwritingQuestionDto>(result);
        }

        public async Task<string> InsertUnderwritingQuestionAsync(RiskSpecificUnderwritingQuestionDto model)
        {
            var result = await _riskSpecificUnderwritingQuestionRepository.AddAsync(_mapper.Map<RiskSpecificUnderwritingQuestion>(model));
            await _riskSpecificUnderwritingQuestionRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemoveUnderwritingQuestionAsync(Guid id)
        {
            var entity = await _riskSpecificUnderwritingQuestionRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _riskSpecificUnderwritingQuestionRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdateUnderwritingQuestionAsync(Guid riskId, RiskSpecificUnderwritingQuestionDto model)
        {
            var UnderwritingQuestionsFromDb = await _riskSpecificUnderwritingQuestionRepository.FindAsync(riskId);
            if (UnderwritingQuestionsFromDb != null)
            {
                model.Id = UnderwritingQuestionsFromDb.Id;
                _mapper.Map<RiskSpecificUnderwritingQuestionDto, RiskSpecificUnderwritingQuestion>(model, UnderwritingQuestionsFromDb);
                UnderwritingQuestionsFromDb.IsActive = true;
                await _riskSpecificUnderwritingQuestionRepository.SaveChangesAsync();
                return UnderwritingQuestionsFromDb.Id.ToString();
            }

            var risk = await _riskRepository.FindAsync(riskId);
            if (risk == null)
            {
                throw new ModelNotFoundException("Risk data not found");
            }

            risk.UnderwritingQuestions = _mapper.Map<RiskSpecificUnderwritingQuestion>(model);
            risk.UnderwritingQuestions.IsActive = true;

            await _riskRepository.SaveChangesAsync();
            return riskId.ToString();
        }
    }
}
