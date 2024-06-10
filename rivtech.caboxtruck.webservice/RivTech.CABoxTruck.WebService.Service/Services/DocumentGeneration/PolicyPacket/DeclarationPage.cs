using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class DeclarationPage : SinglePage, IPage
    {
        public string TemplateName => "PolicyDeclarations.docx";
        public int PageOrder => 2;

        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"policypacket-template/{TemplateName}",
                FormValues = new Dictionary<string, string> { { "InsuredName", "QuoteNumber" } }
            };
            return formData;
        }

        public DeclarationPage(ReportData reportData) : base(reportData)
        {
        }
    }
}
