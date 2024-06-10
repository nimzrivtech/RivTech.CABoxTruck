using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.DataSets
{
    public class DriverInfoOptionsService : IDriverInfoOptionsService
    {
        private readonly IBackgroundCheckOptionRepository _backgroundCheckOptionRepository;
        private readonly IDriverTrainingOptionRepository _driverTrainingOptionRepository;
        private readonly IMvrPullingFrequencyOptionRepository _mvrPullingFrequencyOptionRepository;
        private readonly IMapper _mapper;

        public DriverInfoOptionsService(IBackgroundCheckOptionRepository backgroundCheckOptionRepository, IDriverTrainingOptionRepository driverTrainingOptionRepository,
            IMvrPullingFrequencyOptionRepository mvrPullingFrequencyOptionRepository, IMapper mapper)
        {
            _backgroundCheckOptionRepository = backgroundCheckOptionRepository;
            _driverTrainingOptionRepository = driverTrainingOptionRepository;
            _mvrPullingFrequencyOptionRepository = mvrPullingFrequencyOptionRepository;
            _mapper = mapper;
        }

        public async Task<DriverInfoOptionsDto> GetAllAsync()
        {
            var backgroundCheckOptionsFromDb = await _backgroundCheckOptionRepository.GetAllAsync();
            var driverTrainingOptionsFromDb = await _driverTrainingOptionRepository.GetAllAsync();
            var mvrPullingFrequencyCheckOptionsFromDb = await _mvrPullingFrequencyOptionRepository.GetAllAsync();

            return new DriverInfoOptionsDto
            {
                BackgroundCheckOptions = _mapper.Map<List<EnumerationDTO>>(backgroundCheckOptionsFromDb),
                DriverTrainingOptions = _mapper.Map<List<EnumerationDTO>>(driverTrainingOptionsFromDb),
                MvrPullingFrequencyOptions = _mapper.Map<List<EnumerationDTO>>(mvrPullingFrequencyCheckOptionsFromDb),
            };
        }
    }
}
