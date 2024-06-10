using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IRiskManuscriptsRepository : IRepository<RiskManuscripts, Guid>
    {
        Task CancelManuscript(Guid riskDetailId, long manuscriptIdentifier, DateTime effectiveDate, decimal proratedAmount);
        DateTime GetEndorsementEffectiveDate(RiskManuscripts manuscript);
        Task<(IEnumerable<RiskManuscripts>, PaginationDetails)> GetWithPagingAsync(Expression<Func<RiskManuscripts, bool>> expression, Expression<Func<RiskManuscripts, object>> selector, int pageNumber, int pageSize, string sorting);
    }
}