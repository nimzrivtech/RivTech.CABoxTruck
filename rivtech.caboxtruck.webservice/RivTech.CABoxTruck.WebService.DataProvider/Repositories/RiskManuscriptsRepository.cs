using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskManuscriptsRepository : BaseRepository<RiskManuscripts, Guid>, IRiskManuscriptsRepository
    {
        public RiskManuscriptsRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task CancelManuscript(Guid riskDetailId, long manuscriptIdentifier, DateTime effectiveDate, decimal proratedAmount)
        {
            var riskManuscripts = await _context.Set<RiskManuscripts>().Where(manuscript => manuscript.RiskDetailId == riskDetailId).ToListAsync();
            var manuscript = riskManuscripts.FirstOrDefault(x => x.CreatedDate.Ticks == manuscriptIdentifier);

            if (manuscript != null)
            {
                manuscript.ExpirationDate = effectiveDate;
                manuscript.DeletedDate = effectiveDate;
                manuscript.ProratedPremium = proratedAmount;
                _context.Update(manuscript);
                await _context.SaveChangesAsync();
            }
        }


        public DateTime GetEndorsementEffectiveDate(RiskManuscripts manuscript)
        {
            var riskDetail = _context.RiskDetail.First(x => x.Id == manuscript.RiskDetailId);
            var query = (from manus in _context.Set<RiskManuscripts>().AsNoTracking()
                        join rd in _context.RiskDetail.AsNoTracking() on manus.RiskDetailId equals rd.Id
                        join ph in _context.PolicyHistory.AsNoTracking() on rd.Id.ToString() equals ph.PreviousRiskDetailId 
                        join broker in _context.BrokerInfo.AsNoTracking() on rd.RiskId equals broker.RiskId
                        where manus.Title == manuscript.Title && ph.PolicyStatus == RiskStatus.Active && rd.RiskId == riskDetail.RiskId
                        orderby rd.CreatedDate
                        select new { Id = manus.Id, RiskDetailId = rd.Id, EffectiveDate = rd.EndorsementEffectiveDate ?? broker.EffectiveDate.Value }
                    ).Distinct();

            var first = query.First();

            return first.EffectiveDate;
        }

        public async Task<(IEnumerable<RiskManuscripts>, PaginationDetails)> GetWithPagingAsync(Expression<Func<RiskManuscripts, bool>> expression, Expression<Func<RiskManuscripts, object>> selector, int pageNumber, int pageSize, string sorting)
        {

            var count = _context.Set<RiskManuscripts>().CountAsync(expression);
            int cnt = await count;
            pageNumber = cnt <= 10 ? 1 : pageNumber;

            IQueryable<RiskManuscripts> result;

            if (sorting == "ASC")
            {
                result = _context.Set<RiskManuscripts>()
                                   .Where(expression).OrderBy(selector)
                                   .Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                result = _context.Set<RiskManuscripts>()
                                   .Where(expression).OrderByDescending(selector)
                                   .Skip((pageNumber - 1) * pageSize).Take(pageSize);

            }
                

            var totalPages = (int)Math.Ceiling(cnt / (float)pageSize);

            return (await result.ToListAsync(), new PaginationDetails(pageNumber, totalPages, cnt));
        }
    }
}


