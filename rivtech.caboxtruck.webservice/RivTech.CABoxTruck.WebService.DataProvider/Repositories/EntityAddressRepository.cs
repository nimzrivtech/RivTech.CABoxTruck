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
    public class EntityAddressRepository : BaseRepository<EntityAddress, Guid>, IEntityAddressRepository
    {
        public EntityAddressRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<EntityAddress> GetEntityAddressByAddressId(Guid id)
        {
            return await _context.Set<EntityAddress>()
                                    .Include(i => i.Address)
                                    .Where(x => x.AddressId == id)
                                    .FirstOrDefaultAsync();
        }
    }
}
