using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.DataSets
{
    public class ChameleonIssuesOptionRepository : BaseRepository<ChameleonIssuesOption, short>, IChameleonIssuesOptionRepository
    {
        public ChameleonIssuesOptionRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
