using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class OtherMembersOfHouseholdPage : SinglePage, IPacketPage
    {
        public string TemplateName => "RV028-201806.docx";
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

        public OtherMembersOfHouseholdPage(ReportData reportData) : base(reportData)
        {
        }

        public bool isAttached => true;
    }
}
