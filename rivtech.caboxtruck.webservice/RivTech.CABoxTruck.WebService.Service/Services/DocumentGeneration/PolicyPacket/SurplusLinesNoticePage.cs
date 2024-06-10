using System.Collections.Generic;
using System.Linq;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class SurplusLinesNoticePage : CommonValuesPage
    {
        public SurplusLinesNoticePage(ReportData reportData) : base(reportData)
        {
            FormId = "SurplusLinesNotice";
            PageOrder = 29;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var applicant = ReportData.Risk.NameAndAddress;
            var templateName = GetSurplusLineStates().Contains(applicant?.State) ? applicant?.State : "Not-Available";
            var formVersion = base.SetFormVersion();
            var versionPath = formVersion != null ? $"FormsV{formVersion?.Version}/" : "";
            TemplateName = $"{versionPath}Surplus Lines Notices/{templateName}.docx";

            var bind = ReportData.Risk.Binding;
            var creditedOffice = ReportData.DataSets["CreditedOffice"]
                .FirstOrDefault(bt => bt.Id == bind?.CreditedOfficeId)?.Description;
            var slBroker = ReportData.DataSets["SLBrokers"].FirstOrDefault(d => d.Id == creditedOffice);
            var slInfo = ReportData.DataSets["SLInfo"].FirstOrDefault();
            var stateTax = ReportData.DataSets["StateTax"].FirstOrDefault();
            var _bind = ReportData.Risk.Binding;

            FormValues.Add("SLBrokerCompanyName", slInfo?.SLBroker);
            FormValues.Add("SLBrokerAddressLineOne", $"{slInfo?.SLBrokerAddressLineOne},");
            FormValues.Add("SLBrokerAddressCityStateZip", slInfo?.SLBrokerAddressCityStateZip);
            FormValues.Add("FormSignatory", "");
            FormValues.Add("PolicyState", applicant?.State);

            FormValues.Add("SLBrokerAddress", $"{slInfo?.SLBrokerAddressLineOne}, {slInfo?.SLBrokerAddressCityStateZip}");
            FormValues.Add("SLNumber", _bind?.SurplusLIneNum == null ? slInfo?.LicenseNumber : _bind?.SurplusLIneNum);
            FormValues.Add("SLProducerName", slInfo?.ProducerName);
            FormValues.Add("SLProducerNumber", _bind?.ProducerSLNumber == null ? slInfo?.ProducerSLNumber : _bind?.ProducerSLNumber);
            FormValues.Add("TaxRate", stateTax);
        }

        private static IEnumerable<string> GetSurplusLineStates()
        {
            return new[]
            {
                "AL", "AZ", "CA", "CO", "DE", "GA", "IL", "KY", "MD", "NC",
                "NJ", "NV", "OH", "OR", "PA", "SC", "TN", "TX", "VA", "WA", "MO"
            };
        }

        private string SetAppliedCompanyNameByState(dynamic surplusLinesBroker, string state)
        {
            string slBrokerCompanyName = string.Empty;

            if (surplusLinesBroker?.Id == CreditedOffice.Applied.Description)
            {
                switch (state)
                {
                    case "CA":
                    case "FL":
                    case "MA":
                    case "NY":
                    case "TX":
                    case "WA":
                        slBrokerCompanyName = "Vale Insurance Partners";
                        break;
                    default:
                        slBrokerCompanyName = "Rivington Partners";
                        break;
                }
            } else
            {
                slBrokerCompanyName = surplusLinesBroker?.Description;
            }

            return slBrokerCompanyName;
        }
    }
}