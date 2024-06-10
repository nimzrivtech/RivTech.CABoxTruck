using DocumentFormat.OpenXml.Math;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskAdditionalInterestRepository : BaseRepository<RiskAdditionalInterest, Guid>, IRiskAdditionalInterestRepository
    {
        public RiskAdditionalInterestRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<RiskAdditionalInterest>> GetAllIncludeAsync(Guid riskDetailId)
        {
            return await _context.RiskAdditionalInterest
                                .Include(config => config.Entity)
                                    .ThenInclude(config => config.EntityAddresses)
                                        .ThenInclude(config => config.Address)
                                .Where(x => x.RiskDetailId == riskDetailId)
                                .ToListAsync();
        }

        public async Task<RiskAdditionalInterest> FindByIdIncludeAsync(Guid riskAdditionalInterestId)
        {
            return await _context.RiskAdditionalInterest
                                .Include(config => config.Entity)
                                    .ThenInclude(config => config.EntityAddresses)
                                        .ThenInclude(config => config.Address)
                                .FirstOrDefaultAsync(x => x.Id == riskAdditionalInterestId);
        }

        public async Task<(IEnumerable<RiskAdditionalInterest>, PaginationDetails)> GetWithPagingAsync(Expression<Func<RiskAdditionalInterest, bool>> expression, int pageNumber, int pageSize, string sortField, string order)
        {
            try
            {
                var count = _context.Set<RiskAdditionalInterest>().CountAsync(expression);
                IQueryable<RiskAdditionalInterest> result;

                int cnt = await count;
                pageNumber = cnt <= 10 ? 1 : pageNumber;
                
                 result = _context.Set<RiskAdditionalInterest>()
                                    .Where(expression)
                                    .Include(config => config.Entity)
                                    .ThenInclude(config => config.EntityAddresses)
                                        .ThenInclude(config => config.Address);

                var totalPages = (int)Math.Ceiling(cnt / (float)pageSize);

                

                foreach (var ai in result)
                {
                    ai.AdditionalInterestType = AdditionalInterestType.GetEnumerationById<AdditionalInterestType>(ai.AdditionalInterestTypeId).Description;
                    ai.FullName = ai.Entity.FullName;
                    ai.StreetAddress1 = ai.Entity.EntityAddresses.Select(x => x.Address).FirstOrDefault().StreetAddress1;
                }

                #region Sort
                IEnumerable<RiskAdditionalInterest> listAI = result;
                if (order == "ASC")
                {
                    switch (sortField)
                    {
                        case "entityType":
                            listAI = listAI.OrderBy(x => x.AdditionalInterestType);
                            break;
                        case "name":
                            listAI = listAI.OrderBy(x => x.FullName);
                            break;
                        case "address":
                            listAI = listAI.OrderBy(x => x.StreetAddress1);
                            break;
                        case "effectiveDate":
                            listAI = listAI.OrderBy(x => x.EffectiveDate);
                            break;
                        case "expirationDate":
                            listAI = listAI.OrderBy(x => x.ExpirationDate);
                            break;
                        case "isWaiverOfSubrogation":
                            listAI = listAI.OrderBy(x => x.IsWaiverOfSubrogation);
                            break;
                        case "isPrimaryNonContributory":
                            listAI = listAI.OrderBy(x => x.IsPrimaryNonContributory);
                            break;
                        case "isAutoLiability":
                            listAI = listAI.OrderBy(x => x.IsAutoLiability);
                            break;
                        case "isGeneralLiability":
                            listAI = listAI.OrderBy(x => x.IsGeneralLiability);
                            break;
                        case "is30DayNotice":
                            listAI = listAI.OrderBy(x => x.Is30DayNotice);
                            break;
                        default:
                            listAI = listAI.OrderBy(x => x.AddProcessDate);
                            break;

                    }
                }
                else
                {
                    switch (sortField)
                    {
                        case "entityType":
                            listAI = listAI.OrderByDescending(x => x.AdditionalInterestType);
                            break;
                        case "name":
                            listAI = listAI.OrderByDescending(x => x.FullName);
                            break;
                        case "address":
                            listAI = listAI.OrderByDescending(x => x.StreetAddress1);
                            break;
                        case "effectiveDate":
                            listAI = listAI.OrderByDescending(x => x.EffectiveDate);
                            break;
                        case "expirationDate":
                            listAI = listAI.OrderByDescending(x => x.ExpirationDate);
                            break;
                        case "isWaiverOfSubrogation":
                            listAI = listAI.OrderByDescending(x => x.IsWaiverOfSubrogation);
                            break;
                        case "isPrimaryNonContributory":
                            listAI = listAI.OrderByDescending(x => x.IsPrimaryNonContributory);
                            break;
                        case "isAutoLiability":
                            listAI = listAI.OrderByDescending(x => x.IsAutoLiability);
                            break;
                        case "isGeneralLiability":
                            listAI = listAI.OrderByDescending(x => x.IsGeneralLiability);
                            break;
                        case "is30DayNotice":
                            listAI = listAI.OrderBy(x => x.Is30DayNotice);
                            break;
                        default:
                            listAI = listAI.OrderByDescending(x => x.AddProcessDate);
                            break;

                    }

                }

                #endregion
                listAI = listAI.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return (listAI, new PaginationDetails(pageNumber, totalPages, cnt));
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
