using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.DataSets
{
    public class SaferRatingOptionRepository : BaseRepository<SaferRatingOption, short>, ISaferRatingOptionRepository
    {
        public SaferRatingOptionRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
