using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.DataSets
{
    public class PolicyContactTypesRepository : BaseRepository<PolicyContactTypes, short>, IPolicyContactTypesRepository
    {
        public PolicyContactTypesRepository(AppDbContext context)
            :base(context)
        {
        }
    }
}
