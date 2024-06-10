using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface INameAndAddressService
    {
        Task<NameAndAddressDTO> GetNameAndAddress(Guid id);
        Task<AddressDTO> GetAddress(Guid id);
        Task<NameAndAddressDTO> InsertAsync(NameAndAddressDTO model);
        Task<NameAndAddressDTO> UpdateAsync(NameAndAddressDTO model);
        Task<NameAndAddressDTO> UpdateMidtermAsync(NameAndAddressDTO model);
        Task RemoveAddressAsync(Guid id);
        Task InsertEntityAddressAsync(EntityAddressDTO model);
    }
}
