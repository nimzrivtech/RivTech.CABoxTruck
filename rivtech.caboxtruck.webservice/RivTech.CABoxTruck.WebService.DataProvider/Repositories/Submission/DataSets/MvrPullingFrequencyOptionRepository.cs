using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.DataSets
{
    public class MvrPullingFrequencyOptionRepository : BaseRepository<MvrPullingFrequencyOption, short>, IMvrPullingFrequencyOptionRepository
    {
        public MvrPullingFrequencyOptionRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
