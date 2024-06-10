using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.DataSets
{
    public class TrafficLightRatingOptionRepository : BaseRepository<TrafficLightRatingOption, short>, ITrafficLightRatingOptionRepository
    {
        public TrafficLightRatingOptionRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
