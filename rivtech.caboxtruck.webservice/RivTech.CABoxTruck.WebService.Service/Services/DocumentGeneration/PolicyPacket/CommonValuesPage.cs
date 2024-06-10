using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class CommonValuesPage : SinglePage, IPacketPage
    {
        protected const string DateFormat = "MM/dd/yyyy";

        public CommonValuesPage(ReportData reportData) : base(reportData)
        {
            FormValues = new Dictionary<string, string>();
            TableValues = new Dictionary<string, List<Dictionary<string, string>>>();
        }

        protected Dictionary<string, string> FormValues { get; set; }
        protected Dictionary<string, List<Dictionary<string, string>>> TableValues { get; set; }

        protected string CompanyName { get; set; }
        protected string BusinessName { get; set; }
        protected string BusinessAddress { get; set; }
        protected string PolicyState { get; set; }
        protected string EndorsementEffectiveDate { get; set; }
        protected string PolicyEffectiveDate { get; set; }
        protected string PolicyExpirationDate { get; set; }
        protected RiskCoverageDTO Coverage
        {
            get
            {
                var riskOptions = ReportData.Risk.RiskCoverages;
                var optionId = riskOptions.Count == 1 ? 0 : (ReportData.Risk.Binding?.BindOptionId ?? 1) - 1;
                return riskOptions.Count == 0 ? new RiskCoverageDTO() : ReportData.Risk.RiskCoverages[optionId];
            }
        }
        
        //Usage: All symbols for other option is based on Option 1 expect AL Symbol (selectable in Quote Options)
        protected RiskCoverageDTO Coverage1 => ReportData.Risk.RiskCoverages.Count == 0 ? new RiskCoverageDTO() : ReportData.Risk.RiskCoverages[0];
        protected string OptionId => (ReportData.Risk.Binding?.BindOptionId ?? 1).ToString();

        public List<VehicleDTO> Vehicles
        {
            get
            {
                var vehicles = ReportData.DataSets["PolicyVehicles"].Cast<VehicleDTO>().OrderBy(v => v.VIN).ToList();

                if (IsEndorsement) return vehicles;

                return vehicles.Where(v => v.Options != null && v.Options.Contains(OptionId) && v.IsActive != false && v.IsValidated != false)
                    .OrderBy(v => v.VIN).ToList();
            }
        }

        protected decimal ComprehensivePremium
        {
            get
            {
                if (IsEndorsement) return Vehicles.Sum(v => v.ComprehensivePremium ?? 0);
                return Coverage.RiskCoveragePremium?.ComprehensivePremium ?? 0;
            }
        }

        protected decimal CollisionPremium
        {
            get
            {
                if (IsEndorsement) return Vehicles.Sum(v => v.CollisionPremium ?? 0);
                return Coverage.RiskCoveragePremium?.CollisionPremium ?? 0;
            }
        }

        protected decimal RefrigerationPremium => Vehicles?.Sum(v => v.RefrigerationPremium) ?? 0;

        protected bool HasCargo => Coverage.CargoLimitId > LimitConstants.CargoNoneCoverageId;
        protected bool HasGL => Coverage.GLBILimitId.HasValue && Coverage.GLBILimitId != LimitConstants.GLNoneCoverageId && Coverage.GLBILimitId != LimitConstants.GLNoneCoverageIdv7;
        protected bool HasTRC => Coverage.TrailerInterchangeLimitId > LimitConstants.TrailerNoCoverageId;
        protected bool HasNonOwnedTRC => Coverage.NonOwnedTrailerLimitId > LimitConstants.NonOwnedTrailerNoCoverageId;

        protected bool IsEndorsement => ReportData.Endorsement?.PremiumChange != null;

        protected bool AdditionalInterestChanged(Guid? id)
        {
            var changedInterestIds = ReportData.Endorsement?.AdditionalInterestChanges?.Select(c => c.Id).ToList();
            return changedInterestIds != null && changedInterestIds.Contains(id.GetValueOrDefault());
        }


        public string FormId { get; set; }
        public string TemplateName { get; set; }
        public int PageOrder { get; set; }

        public Form GetData()
        {
            SetFormValues();

            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"policypacket-template/CA/{TemplateName}",
                FormValues = FormValues,
                TableValues = TableValues
            };
            return formData;
        }

        public bool isAttached => true;

        protected virtual void SetFormValues()
        {
            var brokerInfo = ReportData.Risk.BrokerInfo;
            var nameAddress = ReportData.Risk.NameAndAddress;
            string emptyDateValuePlaceHolder = "                      ";

            CompanyName = "Texas Insurance Company";
            PolicyEffectiveDate = brokerInfo?.EffectiveDate?.ToString(DateFormat);
            PolicyExpirationDate = brokerInfo?.ExpirationDate?.ToString(DateFormat);
            BusinessName = nameAddress?.BusinessName;
            BusinessAddress = $"{nameAddress?.BusinessAddress}, {nameAddress?.City}, {nameAddress?.State} {nameAddress?.ZipCode}";
            PolicyState = nameAddress?.State;

            var endorsementEffectiveDate = ReportData.Endorsement?.EffectiveDate == DateTime.MinValue ? DateTime.Now : ReportData.Endorsement?.EffectiveDate;
            EndorsementEffectiveDate = endorsementEffectiveDate?.ToString(DateFormat);

            var formValues = new Dictionary<string, string>
            {
                {"InsuredName", BusinessName},
                {"InsuredName2", BusinessName},
                {"InsuredName3", BusinessName},
                {"BusinessAddress", BusinessAddress},
                {"EffectiveDate", PolicyEffectiveDate},
                {"EffectiveDate2", PolicyEffectiveDate},
                {"ExpirationDate", PolicyExpirationDate},
                {"PolicyPeriod", $"{PolicyEffectiveDate} - {PolicyExpirationDate}"},
                {"PolicyNumber", ReportData.Risk?.PolicyNumber},
                {"PolicyNumber1", ReportData.Risk?.PolicyNumber},
                {"PolicyNumber2", ReportData.Risk?.PolicyNumber},
                {"IssuanceDate", string.IsNullOrEmpty(ReportData.Risk?.PolicyNumber) ? $"{emptyDateValuePlaceHolder}" : DateTime.Now.ToString(DateFormat) }
            };

            FormValues = formValues;

            if (FormId != null)
                SetFormVersion();
        }

        protected virtual FormVersionDTO SetFormVersion()
        {
            var riskForm = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == FormId);

            var quoteDate = ReportData.DataSets["QuoteDate"]?.FirstOrDefault();
            var formVersion = riskForm?.Form?.FormVersions?.Where(fv => quoteDate >= fv.EffectiveDate)?
                    .OrderBy(fv => fv.EffectiveDate)?
                    .FirstOrDefault() ??
                    riskForm?.Form?.FormVersions?
                    .OrderByDescending(fv => fv.EffectiveDate)?
                    .FirstOrDefault();

            if (formVersion != null && (quoteDate != null || !ReportData.Risk.IsRaterVersionLocked))
                TemplateName = formVersion.FilePath;
            else
                formVersion = null;

            return formVersion;
        }

        protected string GetLimitsText(int? id, string dataSetName)
        {
            var text = ReportData.DataSets[dataSetName].FirstOrDefault(d => d.Id == id)?.LimitDisplay;
            return Regex.Replace(text?.ToString() ?? "", @"\s+", " ");
        }

        protected string GetLimitsValue(int? id, string dataSetName)
        {
            return ReportData.DataSets[dataSetName].FirstOrDefault(d => d.Id == id)?.LimitValue;
        }

        protected string GetUpperLimitsText(int? id, string dataSetName)
        {
            var texts = GetLimitsText(id, dataSetName).Replace("$", "").Split("/");
            return texts.Length > 0 ? texts[0].Replace("W", "").Replace("K", ",000") : "";
        }

        protected string GetLowerLimitsText(int? id, string dataSetName)
        {
            var texts = GetLimitsText(id, dataSetName).Replace("$", "").Split("/");
            return texts.Length > 1 ? texts[1] : "";
        }
        protected (string, string) GetLimitsSplitText(int? id, string dataSetName)
        {
            var texts = GetLimitsText(id, dataSetName).Replace("$", "").Split("/");
            return (texts[0], texts[1]);
        }

        protected string GetDeductibleText(int? id, string dataSetName)
        {
            var text = GetLowerLimitsText(id, dataSetName);
            return $"${text?.Replace("DED", "").Trim()}";
        }

        protected string GetSymbolsText(int? id)
        {
            string symbolText = ReportData.DataSets["Symbols"].FirstOrDefault(d => d.Id == id)?.Description ?? "";

            if (symbolText.Contains(")") || symbolText.Contains("-"))
            {
                string pattern = @"^\d+";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(symbolText);

                if (match.Success)
                    return match.Value;
            }

            return symbolText;
        }

        protected void ReplaceFormValues(string key, string value)
        {
            FormValues.Remove(key);
            FormValues.Add(key, value);
        }

        protected string BlanketMessage => "All Persons or Organizations as required by written contract with the Named Insured. The written contract must be signed prior to the date of the \"Accident\".";
    }
}