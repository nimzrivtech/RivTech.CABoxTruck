using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IBrokerCommissionService
    {
        Task<List<BrokerCommissionDTO>> GenerateBrokerCommission(BrokerCommissionInputDTO data);
    }
}
