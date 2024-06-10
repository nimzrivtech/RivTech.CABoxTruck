using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class AdditionalTermRepository : BaseRepository<LvAdditionalTerm, byte>, IAdditionalTermRepository
    {
        public AdditionalTermRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
