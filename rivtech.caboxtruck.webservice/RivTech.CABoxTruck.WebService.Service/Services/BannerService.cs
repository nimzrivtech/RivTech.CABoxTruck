using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _iRepository;
        private readonly IMapper _mapper;

        public BannerService(IBannerRepository iRepository, IMapper mapper)
        {
            _iRepository = iRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BannerDTO>> GetAllAsync()
        {
            var result = await _iRepository.GetAllAsync();
            return _mapper.Map<List<BannerDTO>>(result);
        }
    }
}
