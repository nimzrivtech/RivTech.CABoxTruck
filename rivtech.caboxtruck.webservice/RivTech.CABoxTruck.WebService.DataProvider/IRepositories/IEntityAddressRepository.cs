using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IEntityAddressRepository : IRepository<EntityAddress, Guid>
    {
        Task<EntityAddress> GetEntityAddressByAddressId(Guid id);
    }
}
