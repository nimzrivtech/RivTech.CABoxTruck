using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IPolicyContactsService
    {
        Task<List<PolicyContactsDTO>> GetAllPolicyContacts();
        Task<List<PolicyContactsDTO>> GetAllAssignedToPolicyContacts();
    }
}
