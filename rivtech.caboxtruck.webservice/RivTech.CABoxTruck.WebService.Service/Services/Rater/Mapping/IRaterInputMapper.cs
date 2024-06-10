using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public interface IRaterInputMapper 
    {
        Task<JObject> Map(RiskDTO risk, RiskCoverageDTO riskCov, JObject requestBody);
    }
}
