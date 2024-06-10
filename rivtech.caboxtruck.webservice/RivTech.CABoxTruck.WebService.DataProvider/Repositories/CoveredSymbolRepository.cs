using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class CoveredSymbolRepository : BaseRepository<CoveredSymbol, Int16>, ICoveredSymbolRepository
    {
        public CoveredSymbolRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


