using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.DataSets
{
    public class SubjectivitiesRepository : BaseRepository<Subjectivities, short>, ISubjectivitiesRepository
    {
        public SubjectivitiesRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
