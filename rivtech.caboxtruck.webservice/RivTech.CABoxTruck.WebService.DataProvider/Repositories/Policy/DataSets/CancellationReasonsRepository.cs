using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Policy.DataSets;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Policy.DataSets
{
    public class CancellationReasonsRepository : BaseRepository<CancellationReasons, short>, ICancellationReasonsRepository
    {
        public CancellationReasonsRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
