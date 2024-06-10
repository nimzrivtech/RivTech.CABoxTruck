using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class PaymentPostingService : IPaymentPostingService
    {
        private readonly IPaymentPostingRepository _paymentPostingRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        public PaymentPostingService(IPaymentPostingRepository paymentPostingRepository, IRiskDetailRepository riskDetailRepository)
        {
            _paymentPostingRepository = paymentPostingRepository;
            _riskDetailRepository = riskDetailRepository;
        }

        public async  Task<List<PaymentPostingDTO>> GeneratePaymentPosting(PaymentPostingInputDTO data)
        {
            var paymentPostings = await _paymentPostingRepository.GeneratePaymentPostingAsync(data);

            var riskIds = paymentPostings.Select(x => x.Id).Distinct().ToList();
            var riskDetails = await GetRiskCompanyName(riskIds);

            foreach(var item in paymentPostings)
            {
                var riskDetail = riskDetails.Where(x => x.RiskId == item.Id).FirstOrDefault();
                if (riskDetail != null)
                    item.CompanyName = riskDetail?.Insured?.Entity?.CompanyName;
                    item.CreditedOffice = CreditedOffice.GetEnumerationById<CreditedOffice>(item?.CreditedOfficeId)?.Description;
            }

            return paymentPostings;
        }

        private async Task<List<RiskDetail>> GetRiskCompanyName(List<Guid?> riskIds)
        {
            var retList = new List<RiskDetail>();
            foreach(var riskId in riskIds)
            {
                var riskDetail = await _riskDetailRepository.GetLastRiskDetailByRiskIdAsync(riskId);
                retList.Add(riskDetail);
            }

            return retList;
        }
    }
}
