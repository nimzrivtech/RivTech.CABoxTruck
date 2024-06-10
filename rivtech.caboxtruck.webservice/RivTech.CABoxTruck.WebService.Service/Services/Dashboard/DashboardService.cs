using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO.Dashboard;
using RivTech.CABoxTruck.WebService.Service.IServices.Dashboard;
using System.Linq;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Service.Exceptions;

namespace RivTech.CABoxTruck.WebService.Service.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IRiskRepository _riskRepository;

        public DashboardService(IDashboardRepository dashboardRepository, IRiskRepository riskRepository)
        {
            _dashboardRepository = dashboardRepository;
            _riskRepository = riskRepository;
        }

        public async Task<DashboardPolicySearchDTO> GetRiskByPolicyNumber(string policyNumber)
        {
            var result = await _riskRepository.GetRiskByPolicyNumberAsync(policyNumber);

            if (result == null)
            {
                throw new ModelNotFoundException();
            }

            return new DashboardPolicySearchDTO()
            {
                RiskId = result.Id,
                LatestRiskDetailId = result.RiskDetails.OrderByDescending(x => x.UpdatedDate).FirstOrDefault().Id
            };
        }

        public DashboardCountDto GetDashboardCount()
        {
            var results = _dashboardRepository.GetDashboardCount();
            var dashboardCount = new DashboardCountDto();
            foreach (var result in results)
            {
                if (result.IsRenewal)
                {
                    switch (result.Status)
                    {
                        case "Complete":
                            dashboardCount.RcompleteCount = result.Count;
                            break;
                        case "Declined":
                            dashboardCount.RdeclinedCount = result.Count;
                            break;
                        case "In Research":
                            dashboardCount.RinResearchCount = result.Count;
                            break;
                        case "Incomplete":
                            dashboardCount.RinCompleteCount = result.Count;
                            break;
                        case "Quoted":
                            dashboardCount.RquotedCount = result.Count;
                            break;
                        case "Received":
                            dashboardCount.RreceivedCount = result.Count;
                            break;
                        case "Withdrawn":
                            dashboardCount.RwithdrawnCount = result.Count;
                            break;
                        case "Quote in Progress":
                            dashboardCount.RquoteInProgressCount = result.Count;
                            break;
                        case "Not Accepted":
                            dashboardCount.RnotAcceptedCount = result.Count;
                            break;
                    }
                }
                else
                {
                    switch (result.Status)
                    {
                        case "Complete":
                            dashboardCount.CompleteCount = result.Count;
                            break;
                        case "Declined":
                            dashboardCount.DeclinedCount = result.Count;
                            break;
                        case "In Research":
                            dashboardCount.InResearchCount = result.Count;
                            break;
                        case "Incomplete":
                            dashboardCount.InCompleteCount = result.Count;
                            break;
                        case "Quoted":
                            dashboardCount.QuotedCount = result.Count;
                            break;
                        case "Data Only":
                            dashboardCount.DataOnlyCount = result.Count;
                            break;
                        case "Test Class":
                            dashboardCount.TestClassCount = result.Count;
                            break;
                        case "Received":
                            dashboardCount.ReceivedCount = result.Count;
                            break;
                        case "Withdrawn":
                            dashboardCount.WithdrawnCount = result.Count;
                            break;
                        case "Submission":
                            dashboardCount.SubmissionCount = result.Count;
                            break;
                        case "Active":
                            dashboardCount.ActivePoliciesCount = result.Count;
                            break;
                        case "Canceled":
                            dashboardCount.CancelledPoliciesCount = result.Count;
                            break;
                        case "Quote in Progress":
                            dashboardCount.QuoteInProgressCount = result.Count;
                            break;
                        case "Not Accepted":
                            dashboardCount.NotAcceptedCount = result.Count;
                            break;
                        case "Expired":
                            dashboardCount.ExpiredPoliciesCount = result.Count;
                            break;
                    }
                }
            }
            return dashboardCount;
        }
    }
}
