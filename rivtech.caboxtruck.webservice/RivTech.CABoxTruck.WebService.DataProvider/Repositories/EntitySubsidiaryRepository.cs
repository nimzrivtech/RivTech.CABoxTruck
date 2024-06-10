using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class EntitySubsidiaryRepository : BaseRepository<EntitySubsidiary, Guid>, IEntitySubsidiaryRepository
    {
        public EntitySubsidiaryRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
