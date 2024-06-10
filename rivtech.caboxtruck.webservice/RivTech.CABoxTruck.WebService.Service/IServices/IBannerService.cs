using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IBannerService
    {
        Task<IEnumerable<BannerDTO>> GetAllAsync();
    }
}
