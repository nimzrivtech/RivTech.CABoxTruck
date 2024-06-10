using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics
{
    public interface IRiskSpecificDriverHiringCriteriaRepository : IRepository<RiskSpecificDriverHiringCriteria, Guid>
    {
        Task<RiskSpecificDriverHiringCriteria> GetWithIncludesNoTracking(Guid id);
        Task<List<BackgroundCheckOption>> GetBackgroundCheckOptions(Guid id);
        Task<List<DriverTrainingOption>> GetDriverTrainingOptions(Guid id);
    }
}
