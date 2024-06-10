using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.DataSets
{
    public class DriverTrainingOptionService : IDriverTrainingOptionService
    {
        private readonly IDriverTrainingOptionRepository _driverTrainingOptionRepository;
        private readonly IMapper _mapper;

        public DriverTrainingOptionService(IDriverTrainingOptionRepository driverTrainingOptionRepository, IMapper mapper)
        {
            _driverTrainingOptionRepository = driverTrainingOptionRepository;
            _mapper = mapper;
        }

        public async Task<List<EnumerationDTO>> GetAllAsync()
        {
            var result = await _driverTrainingOptionRepository.GetAllAsync();
            return _mapper.Map<List<EnumerationDTO>>(result);
        }
    }
}
