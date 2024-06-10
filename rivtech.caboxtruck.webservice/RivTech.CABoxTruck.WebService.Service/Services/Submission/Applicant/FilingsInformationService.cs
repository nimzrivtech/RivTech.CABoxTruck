using System;
using System.Threading.Tasks;
using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.Applicant;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.Applicant
{
    public class FilingsInformationService : IFilingsInformationService
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IMapper _mapper;
        private readonly IRiskDetailRepository _riskRepository;

        public FilingsInformationService(IRiskDetailRepository riskRepository,
            IEntityRepository entityRepository,
            IMapper mapper)
        {
            _riskRepository = riskRepository;
            _entityRepository = entityRepository;
            _mapper = mapper;
        }


        public async Task<FilingsInformationDTO> GetAsync(Guid riskId)
        {
            var risk = await _riskRepository.FindAsync(r => r.Id == riskId, r => r.Insured);
            var result = _mapper.Map<FilingsInformationDTO>(risk);
            var entity = await _entityRepository.FindAsync(e => e.Id == risk.Insured.EntityId);
            result.USDOTNumber = entity.USDOTNumber;
            result.McNumber = entity.McNumber;
            return result;
        }

        public async Task<string> UpdateAsync(FilingsInformationDTO model)
        {
            var risk = await _riskRepository.FindAsync(r => r.Id == model.RiskDetailId, r => r.Insured);

            _mapper.Map(model, risk);
            await _riskRepository.SaveChangesAsync();

            var entity = await _entityRepository.FindAsync(e => e.Id == risk.Insured.EntityId);
            entity.USDOTNumber = model.USDOTNumber;
            entity.McNumber = model.McNumber;

            var result = await _entityRepository.SaveChangesAsync();

            return result.ToString();
        }
    }
}