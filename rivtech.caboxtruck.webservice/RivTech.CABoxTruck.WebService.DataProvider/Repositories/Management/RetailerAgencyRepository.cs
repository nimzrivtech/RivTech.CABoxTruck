using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RetailerAgencyRepository : BaseRepository<RetailerAgency, Guid>, IRetailerAgencyRepository
    {
        public RetailerAgencyRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<List<RetailerAgency>> GetRetailerAsync(Guid agencyId)
        {
            return await _context.Set<RetailerAgency>()
                                .Include(i => i.Retailer).ThenInclude(ti => ti.Entity)
                                .Where(x => x.AgencyId == agencyId).ToListAsync();
        }
    }
}


