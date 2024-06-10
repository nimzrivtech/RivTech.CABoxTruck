using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public class ClaimsHistoryInputMapper : IRaterInputMapper
    {
        public const string MODULE_NAME = "ClaimsHistory";

        public Task<JObject> Map(RiskDTO risk, RiskCoverageDTO riskCov, JObject requestBody)
        {
            var claimsHistories = risk?.ClaimsHistory;

            if (claimsHistories.Count == 0 || risk.BusinessDetails.YearsInBusiness == 0)
                return Task.FromResult(requestBody);

            for (int i = 0; i <= claimsHistories.Count - 1; i++)
            {
                var claimHistory = claimsHistories.FirstOrDefault(claimHistory => claimHistory.Order == i);
                var requestClaimsInfo = requestBody[MODULE_NAME]["LiabilityClaimsInfo"];
                if (claimHistory != null)
                {
                    if (i == 0)
                    {
                        requestClaimsInfo["LossRunDateCurrent"] = claimHistory?.LossRunDate?.ToString("MM/dd/yyyy");
                        requestClaimsInfo["LossesGroundUpOrNetCurrent"] = claimHistory?.GroundUpNet;
                        requestClaimsInfo["BIPIPMPIncLossCurrent"] = ((claimHistory?.BiPiPmpIncLossTotal ?? 0) > 0) ? $"{claimHistory?.BiPiPmpIncLossTotal}" : "";
                        requestClaimsInfo["PDIncLossCurrent"] = ((claimHistory?.PropertyDamageIncLoss ?? 0) > 0) ? $"{claimHistory?.PropertyDamageIncLoss}" : "";
                        requestClaimsInfo["BIPIPClaimCountCurrent"] = ((claimHistory?.ClaimCountBiPip ?? 0) > 0) ? $"{claimHistory?.ClaimCountBiPip}" : "";
                        requestClaimsInfo["PDClaimCountCurrent"] = ((claimHistory?.ClaimCountPropertyDamage ?? 0) > 0) ? $"{claimHistory?.ClaimCountPropertyDamage}" : "";
                        requestClaimsInfo["OpenClaimsUnder400Current"] = ((claimHistory?.OpenClaimsUnder500 ?? 0) > 0) ? $"{claimHistory?.OpenClaimsUnder500}" : "";
                    }
                    else
                    {
                        requestClaimsInfo["LossRunDateY" + i] = claimHistory?.LossRunDate?.ToString("MM/dd/yyyy");
                        requestClaimsInfo["LossesGroundUpOrNetY" + i] = claimHistory?.GroundUpNet;
                        requestClaimsInfo["BIPIPMPIncLossY" + i] = ((claimHistory?.BiPiPmpIncLossTotal ?? 0) > 0) ? $"{claimHistory?.BiPiPmpIncLossTotal}" : "";
                        requestClaimsInfo["PDIncLossY" + i] = ((claimHistory?.PropertyDamageIncLoss ?? 0) > 0) ? $"{claimHistory?.PropertyDamageIncLoss}" : "";
                        requestClaimsInfo["BIPIPClaimCountY" + i] = ((claimHistory?.ClaimCountBiPip ?? 0) > 0) ? $"{claimHistory?.ClaimCountBiPip}" : "";
                        requestClaimsInfo["PDClaimCountY" + i] = ((claimHistory?.ClaimCountPropertyDamage ?? 0) > 0) ? $"{claimHistory?.ClaimCountPropertyDamage}" : "";
                        requestClaimsInfo["OpenClaimsUnder400Y" + i] = ((claimHistory?.OpenClaimsUnder500 ?? 0) > 0) ? $"{claimHistory?.OpenClaimsUnder500}" : "";
                    }
                }
            }
            return Task.FromResult(requestBody);
        }
    }
}
