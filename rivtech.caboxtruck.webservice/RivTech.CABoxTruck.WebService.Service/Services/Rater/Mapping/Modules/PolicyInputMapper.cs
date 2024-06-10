using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public class PolicyInputMapper : IRaterInputMapper
    {
        public const string MODULE_NAME = "Policy";

        public Task<JObject> Map(RiskDTO risk, RiskCoverageDTO riskCov, JObject requestBody)
        {
            var policyModule = requestBody[MODULE_NAME];
            policyModule["InceptionDate"] = risk?.BrokerInfo?.EffectiveDate?.ToString("MM/dd/yyyy");
            policyModule["CreditedOffice"] = CreditedOffice.GetEnumerationById<CreditedOffice>(risk?.Binding?.CreditedOfficeId).Description;
            return Task.FromResult(requestBody);
        }
    }
}
