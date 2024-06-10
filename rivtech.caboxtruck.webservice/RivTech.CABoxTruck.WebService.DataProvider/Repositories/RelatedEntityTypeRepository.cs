using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RelatedEntityTypeRepository : BaseRepository<LvRelatedEntityType, byte>, IRelatedEntityTypeRepository
    {
        public RelatedEntityTypeRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
