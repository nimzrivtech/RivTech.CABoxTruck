using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class CAQuoteD1 : SinglePage, IPage
    {
        public CAQuoteD1(ReportData reportData) : base(reportData)
        {
        }

        public string TemplateName => "California Quote D1.docx";
        public int PageOrder => 5;

        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"documenttemplate/Quote/CA/{TemplateName}"
            };
            return formData;
        }
    }
}