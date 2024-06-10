using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public class RequestedCoveragesInputMapper : IRaterInputMapper
    {
        public const string MODULE_NAME = "RequestedCoverages";

        private readonly ILimitsService _limitsService;

        public RequestedCoveragesInputMapper(ILimitsService limitsService)
        {
            _limitsService = limitsService;
        }

        public async Task<JObject> Map(RiskDTO getRiskDto, RiskCoverageDTO rskCov, JObject requestBody)
        {
            var moduleName = MODULE_NAME;
            var getLimitsList = await _limitsService.GetPerStateAsync(getRiskDto?.NameAndAddress?.State, getRiskDto.RaterVersionId);
            var coveragesLists = requestBody[moduleName]["OtherCoverages"];

            string autoBodilyInjury = getLimitsList?.AutoBILimits.Where(x => x.Id == rskCov?.AutoBILimitId).Select(x => x.LimitValue).FirstOrDefault();
            string autoPropertyDamage = getLimitsList?.AutoPDLimits.Where(x => x.Id == rskCov?.AutoPDLimitId).Select(x => x.LimitValue).FirstOrDefault();
            string autoLiabilityDeductible = getLimitsList?.LiabilityDeductibleLimits.Where(x => x.Id == rskCov?.LiabilityDeductibleId).Select(x => x.LimitValue).FirstOrDefault();
            string medicalPaymentsLimit = (getLimitsList?.MedPayLimits.Where(x => x.Id == rskCov?.MedPayLimitId).Select(x => x.LimitValue).FirstOrDefault() ?? "").Replace(",", "");
            string pIPLimit = getLimitsList?.PipLimits.Where(x => x.Id == rskCov?.PIPLimitId).Select(x => x.LimitValue).FirstOrDefault();
            string uMSCLLimit = getLimitsList?.UMBILimits.Where(x => x.Id == rskCov?.UMBILimitId).Select(x => x.LimitValue).FirstOrDefault(); //((string)(ParseListToValue(rskCov?.UMBILimitId, requestBody[moduleName]["AutoLiability"]["UMSCLLimit"]) ?? "")).Replace(",", ""); //UMBI - Uninsured Motorist - BI 
            string uMPDLimit = getLimitsList?.UMPDLimits.Where(x => x.Id == rskCov?.UMPDLimitId).Select(x => x.LimitValue).FirstOrDefault(); // ParseListToValue(rskCov?.UMPDLimitId, requestBody[moduleName]["AutoLiability"]["UMPDLimit"]); //Uninsured Motorist - PD 
            string uIMSCLLimit = getLimitsList?.UIMBILimits.Where(x => x.Id == rskCov?.UIMBILimitId).Select(x => x.LimitValue).FirstOrDefault(); //((string)(ParseListToValue(rskCov?.UIMBILimitId, requestBody[moduleName]["AutoLiability"]["UIMSCLLimit"]) ?? "")).Replace(",", "");  //UiMBI - Underinsured Motorist - BI 
            string uIMPDLimit = getLimitsList?.UIMPDLimits.Where(x => x.Id == rskCov?.UIMPDLimitId).Select(x => x.LimitValue).FirstOrDefault(); // ParseListToValue(rskCov?.UIMPDLimitId, requestBody[moduleName]["AutoLiability"]["UIMPDLimit"]); //Underinsured Motorist - PD // need list

            string symbolsRequested = rskCov?.ALSymbolId?.ParseListToValue(requestBody[moduleName]["AutoLiability"]["SymbolsRequested"]);
            string fireTheftSpecPerils = rskCov?.FireDeductibleId?.ParseListToValue(requestBody[moduleName]["PhysicalDamage"]["FireTheftSpecPerils"]);
            string cOMPDeductible = rskCov?.CompDeductibleId?.ParseListToValue(requestBody[moduleName]["PhysicalDamage"]["COMPDeductible"]);
            string cOLLDeductible = rskCov?.CollDeductibleId?.ParseListToValue(requestBody[moduleName]["PhysicalDamage"]["COLLDeductible"]);
            string gLBodilyInjuryLimits = ((string)rskCov?.GLBILimitId?.ParseListToValue(coveragesLists["GLBodilyInjuryLimits"])).Replace(",", "");
            string cargoCollLimit = ((string)rskCov?.CargoLimitId?.ParseListToValue(coveragesLists["CargoCollLimit"])).Replace(",", "");
            string refrigeratedCargoLimits = ((string)rskCov?.RefCargoLimitId?.ParseListToValue(coveragesLists["RefrigeratedCargoLimits"])).Replace(",", "");

            var alModule = requestBody[moduleName]["AutoLiability"];
            alModule["AutoBodilyInjury"] = (autoBodilyInjury.Contains('/')) ? $"'{autoBodilyInjury}" : autoBodilyInjury;
            alModule["AutoPropertyDamage"] = autoPropertyDamage;
            alModule["AutoLiabilityDeductible"] = autoLiabilityDeductible;
            alModule["MedicalPaymentsLimit"] = medicalPaymentsLimit?.SanitizeStringVal();
            alModule["PIPLimit"] = pIPLimit;
            alModule["UMSCLLimit"] = uMSCLLimit;
            alModule["UMPDLimit"] = uMPDLimit;
            alModule["UIMSCLLimit"] = uIMSCLLimit;
            alModule["UIMPDLimit"] = uIMPDLimit;
            alModule["UMStacking"] = "No"; //FROM excel >> Not on UI. Leave as "N" always
            alModule["SymbolsRequested"] = symbolsRequested;

            var pdModule = requestBody[moduleName]["PhysicalDamage"];
            pdModule["FireTheftSpecPerils"] = string.IsNullOrWhiteSpace(fireTheftSpecPerils) ? "No Coverage" : fireTheftSpecPerils;
            pdModule["COMPDeductible"] = string.IsNullOrWhiteSpace(cOMPDeductible) ? "No Coverage" : cOMPDeductible;
            pdModule["COLLDeductible"] = string.IsNullOrWhiteSpace(cOLLDeductible) ? "No Coverage" : cOLLDeductible;

            var otherCovModule = requestBody[moduleName]["OtherCoverages"];
            otherCovModule["GLBodilyInjuryLimits"] = (gLBodilyInjuryLimits != "NONE") ? $"'{gLBodilyInjuryLimits}" : gLBodilyInjuryLimits;
            otherCovModule["CargoCollLimit"] = cargoCollLimit;
            otherCovModule["RefrigeratedCargoLimits"] = refrigeratedCargoLimits; 

            return requestBody;
        }
    }
}
