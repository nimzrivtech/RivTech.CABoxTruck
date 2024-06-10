using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public class HistoricalCoverageInputMapper : IRaterInputMapper
    {
        public const string MODULE_NAME = "HistoricalCoverage";
        private readonly ICoverageHistoryService _coverageHistoryService;

        public HistoricalCoverageInputMapper(
            ICoverageHistoryService coverageHistoryService
        )
        {
            _coverageHistoryService = coverageHistoryService;
        }

        public async Task<JObject>  Map(RiskDTO risk, RiskCoverageDTO riskCov, JObject requestBody)
        {
            var covHistory = await _coverageHistoryService.GetByIdAsync(risk.RiskHistory.RiskDetailId);

            if (covHistory?.RiskHistories.Count == 0 || risk.BusinessDetails.YearsInBusiness == 0)
                return requestBody;

            var module = requestBody[MODULE_NAME];
            for (int i = 0; i <= 4; i++)
            {
                var _covHistory = covHistory?.RiskHistories.FirstOrDefault(x => x.HistoryYear == i);

                var moduleInsuranceCov = module["HistoricalInsuranceCoverage"];
                if (_covHistory is null)
                    continue;

                if (i == 0)
                {
                    moduleInsuranceCov["PolicyInceptionStartDateCurrent"] = _covHistory.PolicyTermStartDate?.ToString("MM/dd/yyyy");
                    moduleInsuranceCov["PolicyEndDateCurrent"] = _covHistory.PolicyTermEndDate?.ToString("MM/dd/yyyy");
                    moduleInsuranceCov["LiabilityLimitsCurrent"] = ((_covHistory.LiabilityLimits ?? 0) > 0) ? $"{_covHistory.LiabilityLimits}" : "";
                    moduleInsuranceCov["LiabilityDeductibleCurrent"] = ((_covHistory.LiabilityDeductible ?? 0) > 0) ? $"{_covHistory.LiabilityDeductible}" : "";
                    moduleInsuranceCov["PowerUnitsCurrent"] = ((_covHistory.NumberOfPowerUnits ?? 0) > 0) ? $"{_covHistory.NumberOfPowerUnits}" : "";
                    moduleInsuranceCov["NoOfVehiclesCurrent"] = ((_covHistory.NumberOfVehicles ?? 0) > 0) ? $"{_covHistory.NumberOfVehicles}" : "";

                    module["ProjectionsAndHistoricalFigures"]["GrossFleetMileage"] = ((_covHistory?.TotalFleetMileage ?? 0) > 0) ? $"{_covHistory?.TotalFleetMileage}" : "";
                }
                else
                {
                    moduleInsuranceCov["PolicyInceptionStartDateY" + i] = _covHistory.PolicyTermStartDate?.ToString("MM/dd/yyyy");
                    moduleInsuranceCov["PolicyEndDateY" + i] = _covHistory.PolicyTermEndDate?.ToString("MM/dd/yyyy");
                    moduleInsuranceCov["LiabilityLimitsY" + i] = ((_covHistory.LiabilityLimits ?? 0) > 0) ? $"{_covHistory.LiabilityLimits}" : "";
                    moduleInsuranceCov["LiabilityDeductibleY" + i] = ((_covHistory.LiabilityDeductible ?? 0) > 0) ? $"{_covHistory.LiabilityDeductible}" : "";
                    moduleInsuranceCov["PowerUnitsY" + i] = ((_covHistory.NumberOfPowerUnits ?? 0) > 0) ? $"{_covHistory.NumberOfPowerUnits}" : "";
                }
            }

            return requestBody;
        }
    }
}
