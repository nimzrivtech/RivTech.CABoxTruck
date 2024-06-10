using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class LastPage : SinglePage, IPage
    {
        public LastPage(ReportData reportData) : base(reportData)
        {
        }

        public string TemplateName => "Last-Page.docx";
        public int PageOrder => 3;

        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"documenttemplate/Quote/{TemplateName}"
            };
            return formData;
        }
    }
}