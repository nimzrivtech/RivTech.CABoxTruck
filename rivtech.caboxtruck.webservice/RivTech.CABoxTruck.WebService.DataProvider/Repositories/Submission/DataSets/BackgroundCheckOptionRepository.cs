using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.DataSets
{
    public class BackgroundCheckOptionRepository : BaseRepository<BackgroundCheckOption, short>, IBackgroundCheckOptionRepository
    {
        public BackgroundCheckOptionRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
