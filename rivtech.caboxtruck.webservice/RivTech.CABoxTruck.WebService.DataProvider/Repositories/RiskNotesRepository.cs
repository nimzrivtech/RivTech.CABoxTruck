using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskNotesRepository : BaseRepository<RiskNotes, Guid>, IRiskNotesRepository
    {
        public RiskNotesRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


