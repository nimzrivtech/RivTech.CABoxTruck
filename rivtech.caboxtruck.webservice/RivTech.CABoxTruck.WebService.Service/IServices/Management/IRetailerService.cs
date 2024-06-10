using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRetailerService
    {
        Task<List<RetailerDTO>> GetAll();
        Task<List<RetailerDTO>> GetAllByAgency(Guid agencyId);
        Task<RetailerDTO> GetById(Guid retailerId);
        Task<RetailerDTO> UpdateAsync(RetailerDTO resource);
        Task<RetailerDTO> InsertAsync(RetailerDTO resource);
        Task<bool> RemoveAsync(Guid id);
    }
}