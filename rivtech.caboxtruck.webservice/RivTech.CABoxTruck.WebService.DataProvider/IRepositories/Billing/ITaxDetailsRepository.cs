using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing
{
    public interface ITaxDetailsRepository : IRepository<TaxDetailsData, Guid>
    {
    }
}
