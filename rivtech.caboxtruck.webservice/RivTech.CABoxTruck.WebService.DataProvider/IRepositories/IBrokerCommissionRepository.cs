using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IBrokerCommissionRepository
    {
        Task<List<BrokerCommissionDTO>> GenerateBrokerCommissionAsync(BrokerCommissionInputDTO data);
    }
}
