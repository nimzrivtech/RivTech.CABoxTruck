using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class InsuranceBinderPage : CommonValuesPage
    {
        public InsuranceBinderPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Insurance Binder New System.docx";
            PageOrder = 2;
            FormId = "InsuranceBinder";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();
            var formVersion = base.SetFormVersion();
            var versionPath = formVersion != null ? $"FormsV{formVersion?.Version}/" : "";

            FormValues.Add("Insured", BusinessName);
            FormValues.Add("Address", BusinessAddress);
            FormValues.Add("CurrentDate", DateTime.Now.ToString(DateFormat));

            var taxDetail = ReportData.DataSets["TaxDetails"].FirstOrDefault(td => td.Id == Coverage.RiskCoveragePremium?.TaxDetailsId);
            var (slPercent, sfPercent, stampingFeeFlat) = GetTaxPercentage();
            FormValues.Add("TotalPremium", $"${Coverage.RiskCoveragePremium?.Premium ?? 0:N2}");
            FormValues.Add("TotalTaxes", $"${Coverage.RiskCoveragePremium?.TaxesAndStampingFees ?? 0:N2}");

            // Hardcode for OR. Currently the only State with Fire Tax
            // TODO: Future enhancement to support other states with FireTax
            if (PolicyState == "OR") 
            {
                TemplateName = $"{versionPath}OR/Insurance Binder New System.docx";
                var orSLTax = taxDetail?.TotalSLTax ?? 0;
                decimal onePctOfTax = orSLTax / 2.3m;
                decimal totalPremTaxandFees = onePctOfTax * 100m;
                var orFireTax = totalPremTaxandFees * 0.003m;
                FormValues.Add("SurplusLineTax", $"${orSLTax - orFireTax:N2} (2.0%)");
                FormValues.Add("ORFireTax", $"${orFireTax:N2} (0.3%)");
            }
            else if (PolicyState == "MS")
            {
                TemplateName = $"{versionPath}MS/Insurance Binder New System.docx";
                var msSLTax = taxDetail?.TotalSLTax ?? 0;
                decimal onePctOfTax = msSLTax / 7.0m;
                decimal totalPremTaxandFees = onePctOfTax * 100m;
                var msWindpoolFee = totalPremTaxandFees * 0.03m;
                FormValues.Add("SurplusLineTax", $"${msSLTax - msWindpoolFee:N2} (4.0%)");
                FormValues.Add("MSWindpoolFee", $"${msWindpoolFee:N2} (3.0%)");
            }
            else
            {
                FormValues.Add("SurplusLineTax", $"${taxDetail?.TotalSLTax ?? 0:N2} ({slPercent}%)");
            }

            if (stampingFeeFlat != "0")
                FormValues.Add("StampingFee", $"${taxDetail?.TotalStampingFee ?? 0:N2} (Flat)");
            else
                FormValues.Add("StampingFee", $"${taxDetail?.TotalStampingFee ?? 0:N2} ({sfPercent}%)");

            var fees = (Coverage.RiskCoveragePremium?.RiskMgrFeeAL ?? 0) + (Coverage.RiskCoveragePremium?.RiskMgrFeePD ?? 0);
            FormValues.Add("PolicyFees", $"${fees:N2}");
            var totalDue = (Coverage.RiskCoveragePremium?.Premium ?? 0) + (Coverage.RiskCoveragePremium?.TaxesAndStampingFees ?? 0) + fees;
            FormValues.Add("TotalDue", $"${totalDue:N2}");
            FormValues.Add("MinimumEarned", $"{ReportData.Risk.Binding?.MinimumEarned:0.#}%");

            FormValues.Add("ALLimit", GetLimitsText(Coverage.AutoBILimitId, "AutoBILimits"));
            FormValues.Add("ALPremium", $"${Coverage.RiskCoveragePremium?.AutoLiabilityPremium ?? 0:N0}");

            var alManuscriptPremium = Coverage?.RiskCoveragePremium?.ALManuscriptPremium ?? 0;
            if (alManuscriptPremium > 0)
            {
                FormValues.Add("ALManuscriptPremium", $"AL Manuscript Premium: ${alManuscriptPremium:N0}");
            }

            TableValues = new Dictionary<string, List<Dictionary<string, string>>>();
            SetTrailerCoverage();
            SetAutoPhysicalDamage();
            SetGeneralLiability();
            SetMotorTruckCargo();
            SetRefrigeration();
        }

        private void SetAutoPhysicalDamage()
        {
            var apdItems = new List<string>();
            if (Coverage.CollDeductibleId > LimitConstants.CollisionNoCoverageId)
            {
                apdItems.Add($"Collision Deductible: {GetLimitsText(Coverage.CollDeductibleId, "CollisionLimits")}");
                apdItems.Add($"Premium: {CollisionPremium.ToCurrency(0)}");
            }

            if (Coverage.CompDeductibleId > LimitConstants.ComprehensiveNoCoverageId)
            {
                apdItems.Add($"Comprehensive Deductible: {GetLimitsText(Coverage.CompDeductibleId, "ComprehensiveLimits")}");
                apdItems.Add($"Premium: {ComprehensivePremium.ToCurrency(0)}");
            }

            if (Coverage.FireDeductibleId > LimitConstants.ComprehensiveNoCoverageId)
            {
                apdItems.Add($"Fire / Theft Deductible: {GetLimitsText(Coverage.FireDeductibleId, "ComprehensiveLimits")}");
                apdItems.Add($"Premium: {ComprehensivePremium.ToCurrency(0)}");
            }
            var entries = apdItems.Select(apdItem => new Dictionary<string, string> {{"Table_APD_Item", apdItem}}).ToList();
            TableValues.Add("APD_", entries);

            var pdAdditionals = new List<Dictionary<string, string>>();

            var pdManuscriptPremium = Coverage?.RiskCoveragePremium?.PDManuscriptPremium ?? 0;
            if (pdManuscriptPremium > 0)
            {
                pdAdditionals.Add(new Dictionary<string, string> {{"Table_APDAdditional_Item", $"PD Manuscript Premium: ${pdManuscriptPremium:N0}"}});
            }

            var hiredPdPremium = Coverage?.RiskCoveragePremium?.HiredPhysicalDamage ?? 0;
            if ((IsFormSelected("RPCA015") || IsFormSelected("RPCAAC015_1")) && hiredPdPremium > 0)
            {
                pdAdditionals.Add(new Dictionary<string, string> { { "Table_APDAdditional_Item", $"Hired Physical Damage Premium: ${hiredPdPremium:N0}" } });
            }

            var trailerPremium = Coverage?.RiskCoveragePremium?.TrailerInterchange ?? 0;
            if (IsFormSelected("RPCA029") && trailerPremium > 0 && ReportData.Risk.RaterVersionId < 7)
            {
                pdAdditionals.Add(new Dictionary<string, string> { { "Table_APDAdditional_Item", $"Trailer Interchange Premium: ${trailerPremium:N0}" } });
            }

            TableValues.Add("APDAdditional_", pdAdditionals);
        }

        private void SetGeneralLiability()
        {
            var glItems = new List<string>();
            if (Coverage.GLBILimitId.HasValue && Coverage.GLBILimitId != LimitConstants.GLNoneCoverageId
                        && Coverage.GLBILimitId != LimitConstants.GLNoneCoverageIdv7)
            {
                glItems.Add($"Occurrence: ${GetUpperLimitsText(Coverage.GLBILimitId, "GlLimits")}");
                glItems.Add($"Aggregate: ${GetLowerLimitsText(Coverage.GLBILimitId, "GlLimits")}");
                glItems.Add($"Premium: ${Coverage.RiskCoveragePremium?.GeneralLiabilityPremium ?? 0:N0} ");
            }
            var entries = glItems.Select(glItem => new Dictionary<string, string> {{"Table_GL_Item", glItem}}).ToList();
            TableValues.Add("GL_", entries);
        }

        private void SetMotorTruckCargo()
        {
            var mtcItems = new List<string>();
            if (Coverage.CargoLimitId > LimitConstants.CargoNoneCoverageId)
            {
                mtcItems.Add($"Limit: ${GetUpperLimitsText(Coverage.CargoLimitId, "CargoLimits")}");
                mtcItems.Add($"Premium: ${Coverage.RiskCoveragePremium?.CargoPremium ?? 0:N0} ");
            }
            var entries = mtcItems.Select(glItem => new Dictionary<string, string> {{"Table_MTC_Item", glItem}}).ToList();
            TableValues.Add("MTC_", entries);
        }

        private void SetRefrigeration()
        {
            var refItems = new List<string>();
            if (Coverage.RefCargoLimitId > LimitConstants.RefNoneCoverageId)
            {
                refItems.Add($"Limit: ${GetUpperLimitsText(Coverage.RefCargoLimitId, "RefLimits")}");
                refItems.Add($"Premium: ${RefrigerationPremium:N0} ");
            }
            var entries = refItems.Select(refItem => new Dictionary<string, string> {{"Table_Ref_Item", refItem}}).ToList();
            TableValues.Add("Ref_", entries);
        }

        private void SetTrailerCoverage()
        {
            var trcItems = new List<string>();
            if (Coverage.TrailerInterchangeLimitId > LimitConstants.TrailerNoCoverageId)
            {
                var limitValue = ParseTrailerLimitValue(GetLimitsValue(Coverage.TrailerInterchangeLimitId, "TrailerInterchangeLimits"));
                trcItems.Add($"Limit: ${limitValue.Item1}");
                trcItems.Add($"Deductible: ${limitValue.Item2}");
                trcItems.Add($"Premium: ${Coverage.RiskCoveragePremium?.TrailerInterchangeAL ?? 0:N0}");
            }

            var entriesTRC = trcItems.Select(trcItem => new Dictionary<string, string> { { "Table_TRC_Item", trcItem } }).ToList();
            TableValues.Add("TRC_", entriesTRC);

            var nonTRCItems = new List<string>();
            if (Coverage.NonOwnedTrailerLimitId > LimitConstants.NonOwnedTrailerNoCoverageId)
            {
                var limitValue = ParseTrailerLimitValue(GetLimitsValue(Coverage.NonOwnedTrailerLimitId, "NonOwnedTrailerLimits"));
                nonTRCItems.Add($"Limit: ${limitValue.Item1}");
                nonTRCItems.Add($"Deductible: ${limitValue.Item2}");
                nonTRCItems.Add($"Premium: ${Coverage.RiskCoveragePremium?.NonOwnedTrailerPremium ?? 0:N0}");
            }

            var entriesNONTRC = nonTRCItems.Select(nontrcItem => new Dictionary<string, string> { { "Table_NONTRC_Item", nontrcItem } }).ToList();
            TableValues.Add("NONTRC_", entriesNONTRC);
        }

        private bool IsFormSelected(string formNumber)
        {
            var riskForm = ReportData.Risk.RiskForms?.FirstOrDefault(rf =>
                rf.Form?.FormNumber == formNumber && rf.IsSelected.GetValueOrDefault());
            return riskForm != null;
        }

        private Tuple<string, string> ParseTrailerLimitValue(string trailerLimitValue = "")
        {
            string pattern = @"(\d+) WITH (\d+)";

            MatchCollection matches = Regex.Matches(trailerLimitValue, pattern);
            var matched = matches.FirstOrDefault();

            if (matched != null)
            {
                var value1 = matched?.Groups[1]?.Value;
                var value2 = matched?.Groups[2]?.Value;

                if (value1 != null)
                    value1 = int.Parse(matched.Groups[1].Value).ToString("N0");

                if (value2 != null)
                    value2 = int.Parse(matched.Groups[2].Value).ToString("N0");

                return Tuple.Create(value1 ?? "0", value2 ?? "0");
            }

            return Tuple.Create("", "");
        }

        private (string taxPercent, string stampingFeePercent, string stampingFeeFlat) GetTaxPercentage()
        {
            var taxPercent = "0";
            var stampingFeePercent = "0";
            var stampingFeeFlat = "0";

            switch (PolicyState)
            {
                case "CA":
                    taxPercent = "3";
                    stampingFeePercent = "0.25";
                    if (DateTime.Parse(PolicyEffectiveDate) >= new DateTime(2023, 1, 1))
                        stampingFeePercent = "0.18";
                    break;
                case "NJ":
                    taxPercent = "5";
                    stampingFeePercent = "0";
                    break;
                case "TX":
                    taxPercent = "4.85";
                    stampingFeePercent = "0.075";

                    if (DateTime.Parse(PolicyEffectiveDate) >= new DateTime(2024, 1, 1))
                        stampingFeePercent = "0.040";
                    break;
                case "GA":
                    taxPercent = "4";
                    stampingFeePercent = "0";
                    break;
                case "MO":
                    taxPercent = "5";
                    stampingFeePercent = "0";
                    break;
                case "PA":
                    taxPercent = "3";
                    stampingFeePercent = "0";
                    stampingFeeFlat = "20.00";
                    break;
                case "FL":
                    taxPercent = "5";
                    stampingFeePercent = "0.1";
                    break;
                case "OR":
                    taxPercent = "2.0";
                    stampingFeePercent = "0";
                    stampingFeeFlat = "10.00";
                    break;
                case "OH":
                    taxPercent = "5";
                    stampingFeePercent = "0";
                    break;
                case "CO":
                    taxPercent = "3";
                    stampingFeePercent = "0";
                    break;
                case "IN":
                    taxPercent = "2.5";
                    stampingFeePercent = "0";
                    break;
                case "VA":
                    taxPercent = "2.25";
                    stampingFeePercent = "0";
                    break;
                case "TN":
                    taxPercent = "5";
                    stampingFeePercent = "0.175";
                    break;
                case "IL":
                    taxPercent = "3.5";
                    stampingFeePercent = "0.04";
                    break;
                case "AZ":
                    taxPercent = "3";
                    stampingFeePercent = "0.2";
                    break;
                case "MS":
                    taxPercent = "7";
                    stampingFeePercent = "0.25";
                    break;
                case "SC":
                    taxPercent = "6";
                    stampingFeePercent = "0";
                    break;
                case "AL":
                    taxPercent = "6";
                    stampingFeePercent = "0";
                    break;
            }
            return (taxPercent, stampingFeePercent, stampingFeeFlat);
        }
    }
}