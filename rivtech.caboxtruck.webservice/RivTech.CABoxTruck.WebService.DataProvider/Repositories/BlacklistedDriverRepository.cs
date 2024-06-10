using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class BlacklistedDriverRepository: BaseRepository<BlacklistedDriver, Guid>, IBlacklistedDriverRepository
    {
        public BlacklistedDriverRepository(AppDbContext context) : base(context)
        {
            
        }

        public IQueryable<BlacklistedDriver> GetAll()
        {
            return _context.Set<BlacklistedDriver>().AsQueryable();
        }
    }
}
