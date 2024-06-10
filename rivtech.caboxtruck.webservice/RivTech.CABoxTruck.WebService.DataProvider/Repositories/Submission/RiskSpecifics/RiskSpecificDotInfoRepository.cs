using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.RiskSpecifics
{
    public class RiskSpecificDotInfoRepository : BaseRepository<RiskSpecificDotInfo, Guid>, IRiskSpecificDotInfoRepository
    {
        public RiskSpecificDotInfoRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}
