using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskTypeRepository : BaseRepository<LvRiskType, byte>, IRiskTypeRepository
    {
        public RiskTypeRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
