using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Submission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission
{
    public class RiskRepository : BaseRepository<Risk, Guid>, IRiskRepository
    {
        public RiskRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<RiskSummaryDto>> GetFilteredSubmissionSummaryListAsync(SubmissionFilterDto filter)
        {
            var risks = await _context.Set<Risk>().Include(risk => risk.RiskDetails).ThenInclude(riskDetail => riskDetail.Insured).ThenInclude(insured => insured.Entity).ToListAsync();
            var result = new List<RiskSummaryDto>();
            foreach(var item in risks)
            {
                var riskSummary = new RiskSummaryDto();
                // Policy Submission
                if (item.RiskDetails.Count() > 1 && item.Status != RiskStatus.Pending)
                {
                    //var issued = item.RiskDetails.OrderByDescending(x => x.UpdatedDate).Where(x => x.Status == RiskDetailStatus.Issued).FirstOrDefault();
                    var latestEndorsement = item.RiskDetails.OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                    
                    if (latestEndorsement != null)
                    {
                        var riskStatus = item.Status == RiskDetailStatus.Expired ? RiskDetailStatus.Expired : RiskDetailStatus.Active;

                        riskSummary.Id = Guid.NewGuid();
                        riskSummary.CurrentRiskId = item.ForEndorsementRiskDetailId ?? Guid.Empty;
                        riskSummary.SubmissionNumber = item.SubmissionNumber;
                        riskSummary.PolicyNumber = item.PolicyNumber; 
                        riskSummary.Status = latestEndorsement.Status == RiskDetailStatus.Canceled ? RiskDetailStatus.Canceled : riskStatus;
                        riskSummary.InsuredName = latestEndorsement.Insured.Entity.CompanyName;
                        riskSummary.InsuredDBA = latestEndorsement.Insured.Entity.DBA;
                        riskSummary.InsuredEntityId = latestEndorsement.Insured.EntityId;
                        riskSummary.NewVenture = latestEndorsement.IsNewVenture.ToYesNo();
                        riskSummary.UseClass = latestEndorsement.MainUseId;
                        riskSummary.DateSubmitted = latestEndorsement.CreatedDate;
                        riskSummary.RiskId = latestEndorsement.RiskId;
                        riskSummary.Owner = latestEndorsement.Insured.Entity.BusinessPrincipal;
                        riskSummary.PolicyStatus = item.Status;
                        riskSummary.IsPolicy = true;
                        riskSummary.CreatedDate =
                            item.RiskDetails.OrderBy(x => x.CreatedDate).FirstOrDefault().CreatedDate;
                        riskSummary.AssignedToId = latestEndorsement.AssignedToId.HasValue ? latestEndorsement.AssignedToId.Value.ToString() : null;
                        riskSummary.IsRenewal = item.IsRenewal;
                        riskSummary.SubmissionType = item.IsRenewal ? "Renewal": "New";
                        result.Add(riskSummary);
                        continue;
                    }
                }

                // Submission
                riskSummary = new RiskSummaryDto
                {
                    Id = Guid.NewGuid(),
                    CurrentRiskId = item.RiskDetails[0].Id,
                    SubmissionNumber = item.SubmissionNumber,
                    PolicyNumber = item.PolicyNumber,
                    Status = item.RiskDetails[0].Status,
                    PolicyStatus = item.Status,
                    InsuredName = item.RiskDetails[0].Insured.Entity.CompanyName,
                    InsuredDBA = item.RiskDetails[0].Insured.Entity.DBA,
                    InsuredEntityId = item.RiskDetails[0].Insured.EntityId,
                    NewVenture = item.RiskDetails[0].IsNewVenture.ToYesNo(),
                    UseClass = item.RiskDetails[0].MainUseId,
                    DateSubmitted = item.RiskDetails[0].CreatedDate,
                    RiskId = item.RiskDetails[0].RiskId,
                    Owner = item.RiskDetails[0].Insured.Entity.BusinessPrincipal,
                    CreatedDate = item.RiskDetails.OrderBy(x => x.CreatedDate).FirstOrDefault().CreatedDate,
                    AssignedToId = item.RiskDetails[0].AssignedToId.HasValue ? item.RiskDetails[0].AssignedToId.Value.ToString() : null,
                    IsRenewal = item.IsRenewal,
                    SubmissionType = item.IsRenewal ? "Renewal" : "New"
                };
                result.Add(riskSummary);
            }

            return result.AsEnumerable();
        }

        public async Task<Risk> GetRiskByPolicyNumberAsync(string policyNumber)
        {
            return _context.Set<Risk>()
                        .Where(x => x.PolicyNumber == policyNumber && (x.Status == RiskStatus.Active || x.Status == RiskStatus.Canceled))
                        .Include(risk => risk.RiskDetails)
                        .ThenInclude(riskDetail => riskDetail.Insured)
                        .ThenInclude(insured => insured.Entity)
                        .ThenInclude(config => config.EntityAddresses).FirstOrDefault();
        }

        public async Task<List<Guid>> GetAllRiskIdForCancellationAsync(DateTime pendingCancellationDate)
        {
            return await _context.Set<Risk>()
                        .Where(x => x.PendingCancellationDate.Value.Date <= pendingCancellationDate.Date && x.Status == RiskStatus.Active)
                        .Select(x => x.Id).ToListAsync();
        }

        public async Task<List<Risk>> GetAllRiskIdForPendingCancellationAsync()
        {
            return await _context.Set<Risk>()
                        .Where(x => x.PendingCancellationDate != null && x.Status == RiskStatus.Active && x.IsPendingCancellation == true).ToListAsync();
        }
        
        public async Task<Risk> GetRiskByIdAsync(Guid id)
        {
            return await _context.Set<Risk>()
                        .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Risk> GetByIdNoTrackingAsync(Guid id)
        {
            return await _context.Set<Risk>().AsNoTracking()
                        .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Risk> GetRiskForEndorsement(Guid id)
        {
            return await _context.Set<Risk>()
                        .Include(x => x.CurrentRiskDetail)
                        .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Risk>> GetManualNoticeCancellationRisks(DateTime noticeDate)
        {
            return await _context.Set<Risk>()
                                .Where(x => x.PendingCancellationNoticeDate.HasValue && x.PendingCancellationNoticeDate <= noticeDate && x.IsPendingCancellation == false).ToListAsync();
        }

        public async Task<Risk> GetBySubmissionAsync(string submission)
        {
            return await _context.Set<Risk>().Where(x => x.SubmissionNumber == submission).SingleOrDefaultAsync();
        }

        public async Task<string[]> GetForRenewalsAsync(DateTime date)
        {
            var activeRisks = await _context.Set<Risk>().AsSplitQuery()
                           .Include(x => x.BrokerInfo).AsSplitQuery()
                           .Where(r => r.Status == RiskStatus.Active) 
                           .ToListAsync();
            var forRenewals = activeRisks.Where(r => r.IsForRenewal(date));
            return forRenewals.Select(x =>  x.PolicyNumber).ToArray();
        }

        public async Task<string[]> GetExpiredPoliciesAsync(DateTime date)
        {
            var expiredNoPending = new List<string>();
            var activeRisks = await _context.Set<Risk>() //.AsSplitQuery()
                            .Include(x => x.RiskDetails) //.AsSplitQuery()
                            .Include(x => x.BrokerInfo) //.AsSplitQuery()
                           .Where(r => r.Status == RiskStatus.Active)
                           .ToListAsync();
            var expiredList = activeRisks.Where(r => r.IsForExpired(date));
            foreach (var expired in expiredList)
            {
                var hasPendingEndorsement = expired.RiskDetails
                    .Any(x => x.Status == RiskDetailStatus.PendingEndorsement);
                if (!hasPendingEndorsement)
                    expiredNoPending.Add(expired.PolicyNumber);
            }

            return expiredNoPending.ToArray();
        }

        public async Task<List<FieldVersion>> GetFieldVersions()
        {
            return await _context.Set<FieldVersion>().ToListAsync();
        }
    }
}
