
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class BannerRepository : BaseRepository<Banner, Guid>,  IBannerRepository
    {
        public BannerRepository(AppDbContext context)
            : base(context)
        {
        }

        public override async Task<IEnumerable<Banner>> GetAllAsync()
        {
            return await _context.Set<Banner>().Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).Take(7).ToListAsync();
        }
    }
}
