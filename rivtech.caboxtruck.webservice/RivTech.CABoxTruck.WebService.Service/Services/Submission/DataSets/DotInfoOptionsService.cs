using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.DataSets
{
    public class DotInfoOptionsService : IDotInfoOptionsService
    {
        private readonly IChameleonIssuesOptionRepository _chameleonIssuesOptionRepository;
        private readonly ISaferRatingOptionRepository _saferRatingOptionRepository;
        private readonly ITrafficLightRatingOptionRepository _trafficLightRatingOptionRepository;
        private readonly IMapper _mapper;

        public DotInfoOptionsService(IChameleonIssuesOptionRepository chameleonIssuesOptionRepository, ISaferRatingOptionRepository saferRatingOptionRepository,
            ITrafficLightRatingOptionRepository trafficLightRatingOptionRepository, IMapper mapper)
        {
            _chameleonIssuesOptionRepository = chameleonIssuesOptionRepository;
            _saferRatingOptionRepository = saferRatingOptionRepository;
            _trafficLightRatingOptionRepository = trafficLightRatingOptionRepository;
            _mapper = mapper;
        }

        public async Task<DotInfoOptionsDto> GetAllAsync()
        {
            var chameleonIssuesOptionsFromDb = await _chameleonIssuesOptionRepository.GetAllAsync();
            var saferRatingOptionsFromDb = await _saferRatingOptionRepository.GetAllAsync();
            var trafficLightRatingCheckOptionsFromDb = await _trafficLightRatingOptionRepository.GetAllAsync();

            return new DotInfoOptionsDto
            {
                ChameleonIssuesOptions = _mapper.Map<List<EnumerationDTO>>(chameleonIssuesOptionsFromDb),
                SaferRatingOptions = _mapper.Map<List<EnumerationDTO>>(saferRatingOptionsFromDb),
                TrafficLightRatingOptions = _mapper.Map<List<EnumerationDTO>>(trafficLightRatingCheckOptionsFromDb),
            };
        }
    }
}
