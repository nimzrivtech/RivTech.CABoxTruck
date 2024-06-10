using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskStatusTypeRepository : BaseRepository<LvRiskStatusType, byte>, IRiskStatusTypeRepository
    {
        public RiskStatusTypeRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
