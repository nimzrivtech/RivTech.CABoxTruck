using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class BrokerCommissionService : IBrokerCommissionService
    {
        private readonly IBrokerCommissionRepository _brokerCommissionRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IBindingRepository _bindingRepository;
        public BrokerCommissionService(IBrokerCommissionRepository brokerCommissionRepository, IRiskDetailRepository riskDetailRepository, IBindingRepository bindingRepository)
        {
            _brokerCommissionRepository = brokerCommissionRepository;
            _riskDetailRepository = riskDetailRepository;
            _bindingRepository = bindingRepository;
        }

        public async Task<List<BrokerCommissionDTO>> GenerateBrokerCommission(BrokerCommissionInputDTO data)
        {
            var brokerCommissions = await _brokerCommissionRepository.GenerateBrokerCommissionAsync(data);

            var riskIds = brokerCommissions.Select(x => x.Id).Distinct().ToList();
            var riskDetails = await GetRiskCompanyName(riskIds);

            foreach (var item in brokerCommissions)
            {
                var riskDetail = riskDetails.Where(x => x.RiskId == item.Id).FirstOrDefault();

                if (riskDetail != null)
                {
                    var binding = await _bindingRepository.GetByRiskAsync(riskDetail.RiskId);

                    item.CompanyName = riskDetail?.Insured?.Entity?.CompanyName;
                    item.CreditedOffice = CreditedOffice.GetEnumerationById<CreditedOffice>(binding?.CreditedOfficeId)?.Description;
                }
            }

            return brokerCommissions;
        }

        private async Task<List<RiskDetail>> GetRiskCompanyName(List<Guid?> riskIds)
        {
            var retList = new List<RiskDetail>();
            foreach (var riskId in riskIds)
            {
                var riskDetail = await _riskDetailRepository.GetLastRiskDetailByRiskIdAsync(riskId);
                retList.Add(riskDetail);
            }

            return retList;
        }
    }
}
