using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class CAQuoteUM : SinglePage, IPage
    {
        public CAQuoteUM(ReportData reportData) : base(reportData)
        {
        }

        public string TemplateName => "California Quote UM Form Coverage Selection.docx";
        public int PageOrder => 4;

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
                {"InsuredName", nameAddress?.BusinessName}
            };
            return formValues;
        }
    }
}