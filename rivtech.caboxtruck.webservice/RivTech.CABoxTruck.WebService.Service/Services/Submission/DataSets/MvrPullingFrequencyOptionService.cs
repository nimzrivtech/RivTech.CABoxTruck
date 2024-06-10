using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.DataSets
{
    public class MvrPullingFrequencyOptionService : IMvrPullingFrequencyOptionService
    {
        private readonly IMvrPullingFrequencyOptionRepository _mvrPullingFrequencyOptionRepository;
        private readonly IMapper _mapper;

        public MvrPullingFrequencyOptionService(IMvrPullingFrequencyOptionRepository mvrPullingFrequencyOptionRepository, IMapper mapper)
        {
            _mvrPullingFrequencyOptionRepository = mvrPullingFrequencyOptionRepository;
            _mapper = mapper;
        }

        public async Task<List<EnumerationDTO>> GetAllAsync()
        {
            var result = await _mvrPullingFrequencyOptionRepository.GetAllAsync();
            return _mapper.Map<List<EnumerationDTO>>(result);
        }
    }
}