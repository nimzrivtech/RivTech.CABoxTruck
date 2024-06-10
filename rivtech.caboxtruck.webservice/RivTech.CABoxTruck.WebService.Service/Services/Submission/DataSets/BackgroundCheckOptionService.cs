using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.DataSets
{
    public class BackgroundCheckOptionService : IBackgroundCheckOptionService
    {
        private readonly IBackgroundCheckOptionRepository _backgroundCheckOptionRepository;
        private readonly IMapper _mapper;

        public BackgroundCheckOptionService(IBackgroundCheckOptionRepository backgroundCheckOptionRepository, IMapper mapper)
        {
            _backgroundCheckOptionRepository = backgroundCheckOptionRepository;
            _mapper = mapper;
        }

        public async Task<List<EnumerationDTO>> GetAllAsync()
        {
            var result = await _backgroundCheckOptionRepository.GetAllAsync();
            return _mapper.Map<List<EnumerationDTO>>(result);
        }
    }
}
