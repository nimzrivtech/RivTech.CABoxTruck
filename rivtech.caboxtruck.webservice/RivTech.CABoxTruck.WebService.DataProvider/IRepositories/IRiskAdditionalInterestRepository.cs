using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IRiskAdditionalInterestRepository : IRepository<RiskAdditionalInterest, Guid>
    {
        Task<IEnumerable<RiskAdditionalInterest>> GetAllIncludeAsync(Guid riskDetailId);
        Task<RiskAdditionalInterest> FindByIdIncludeAsync(Guid riskAdditionalInterestId);
        Task<(IEnumerable<RiskAdditionalInterest>, PaginationDetails)> GetWithPagingAsync(Expression<Func<RiskAdditionalInterest, bool>> expression, int pageNumber, int pageSize, string sortField, string order);
    }
}
