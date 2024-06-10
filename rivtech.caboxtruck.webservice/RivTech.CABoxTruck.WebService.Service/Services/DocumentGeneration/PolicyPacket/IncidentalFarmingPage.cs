using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class IncidentalFarmingPage : SinglePage, IPacketPage
    {
        public string TemplateName => "RV010-201806.docx";
        public int PageOrder { get; }
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

        public IncidentalFarmingPage(ReportData reportData) : base(reportData)
        {
        }

        public bool isAttached => true;
    }
}