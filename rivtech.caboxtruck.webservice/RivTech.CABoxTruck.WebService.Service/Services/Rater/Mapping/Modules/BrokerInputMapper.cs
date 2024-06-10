using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public class BrokerInputMapper : IRaterInputMapper
    {
        public const string MODULE_NAME = "Broker";

        public Task<JObject> Map(RiskDTO risk, RiskCoverageDTO riskCov, JObject requestBody)
        {
            var brkr = risk?.BrokerInfo;
            var brokerModule = requestBody[MODULE_NAME];
            brokerModule["EffectiveDate"] = brkr?.EffectiveDate?.ToString("MM/dd/yyyy");
            brokerModule["CommissionAL"] = !string.IsNullOrEmpty(riskCov.BrokerCommisionAL) ? $"{(Convert.ToDecimal(riskCov.BrokerCommisionAL)) / 100}" : string.Empty;
            brokerModule["CommissionPD"] = !string.IsNullOrEmpty(riskCov.BrokerCommisionPD) ? $"{(Convert.ToDecimal(riskCov.BrokerCommisionPD)) / 100}" : string.Empty;
            return Task.FromResult(requestBody);
        }
    }
}
