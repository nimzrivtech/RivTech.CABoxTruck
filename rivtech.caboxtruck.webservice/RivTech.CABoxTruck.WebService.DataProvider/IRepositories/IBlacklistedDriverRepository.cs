using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IBlacklistedDriverRepository: IRepository<BlacklistedDriver, Guid>
    {
        IQueryable<BlacklistedDriver> GetAll();
    }
}
