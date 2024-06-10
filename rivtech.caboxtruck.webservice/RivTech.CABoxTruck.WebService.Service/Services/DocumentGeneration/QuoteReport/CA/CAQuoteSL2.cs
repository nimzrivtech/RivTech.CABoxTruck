using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class CAQuoteSL2 : SinglePage, IPage
    {
        public CAQuoteSL2(ReportData reportData) : base(reportData)
        {
        }

        public string TemplateName => "California Quote SL2.docx";
        public int PageOrder => 6;

        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"documenttemplate/Quote/CA/{TemplateName}",
                FormValues = GetFormValues()
            };
            return formData;
        }

        private Dictionary<string, string> GetFormValues()
        {
            var nameAddress = ReportData.Risk.NameAndAddress;

            var formValues = new Dictionary<string, string>
            {
                {"InsuredName", nameAddress?.BusinessName},
                {"Street", nameAddress?.BusinessAddress},
                {"City", nameAddress?.City},
                {"State", nameAddress?.State},
                {"ZipCode", nameAddress?.ZipCode},
            };
            return formValues;
        }
    }
}