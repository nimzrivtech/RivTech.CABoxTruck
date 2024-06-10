using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Billing
{
    public class TaxDetailsRepository : BaseRepository<TaxDetailsData, Guid>, ITaxDetailsRepository
    {
        public TaxDetailsRepository(AppDbContext dbContext) : base(dbContext) { }

    }
}
