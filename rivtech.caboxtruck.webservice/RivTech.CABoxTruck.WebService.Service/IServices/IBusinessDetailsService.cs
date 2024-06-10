using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IBusinessDetailsService
    {
        Task<BusinessDetailsDTO> GetBusinessDetails(Guid id);
        Task<BusinessDetailsDTO> InsertUpdateBusinessDetailsAsync(BusinessDetailsDTO model);
    }
}
