using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class QuoteVerification : SinglePage, IPage
    {
        public QuoteVerification(ReportData reportData) : base(reportData)
        {
        }

        protected const string DateFormat = "MM/dd/yyyy";
        protected string BusinessName { get; set; }
        protected string BusinessAddress { get; set; }
        protected string PolicyEffectiveDate { get; set; }


        public string TemplateName => "QuoteVerification.docx";
        public int PageOrder => 2;

        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"documenttemplate/Quote/{TemplateName}",
                FormValues = GetFormValues(),
            };
            return formData;
        }

        private Dictionary<string, string> GetFormValues()
        {
            var brokerInfo = ReportData.Risk.BrokerInfo;
            var nameAddress = ReportData.Risk.NameAndAddress;
            var businessDetails = ReportData.Risk.NameAndAddress;
            var usDotNumber = ReportData.Risk?.FilingsInformation?.USDOTNumber;
            var assignedTo = ReportData.Risk?.BrokerInfo?.Agent?.Entity?.FullName;
            var federalIdNumber = ReportData.Risk?.BusinessDetails?.FederalIDNumber;
            var quotePolicyNumber = ReportData.Risk?.PolicyNumber;

            PolicyEffectiveDate = brokerInfo?.EffectiveDate?.ToString(DateFormat);
            BusinessName = nameAddress?.BusinessName;
            BusinessAddress = $"{nameAddress?.BusinessAddress}, {nameAddress?.City}, {nameAddress?.State} {nameAddress?.ZipCode}";

            var formValues = new Dictionary<string, string>
            {
                {"InsuredName", BusinessName},
                {"InsuredName2", BusinessName},
                {"BusinessAddress", BusinessAddress},
                {"FEIN", federalIdNumber},
                {"Phone", businessDetails.PhoneExt},
                {"Email", businessDetails.Email},
                {"AssignedTo", assignedTo},
                {"QuotePolicyNumber", quotePolicyNumber},
                {"EffectiveDate", PolicyEffectiveDate},
                {"UsDotNumber", usDotNumber},
            };
            return formValues;
        }
    }
}