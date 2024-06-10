using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class EntityRepository : BaseRepository<Entity, Guid>, IEntityRepository
    {
        public EntityRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<Entity> GetEntitiesAsync(Guid id)
        {
            return await _context.Entity.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
