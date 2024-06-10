using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PolicyPacketPageFactory
    {
        // put this to work
        //https://medium.com/@sandeep.singh2632/factory-design-pattern-b51218ac378f

        private const string PACKETNAMESPACE = "RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket";

        public Dictionary<string, IPacketPage> Factories { get; }
        public ReportData ReportData { get; set; }

        public PolicyPacketPageFactory(Guid riskDetailId, ReportData reportData, IEnumerable<PageInfo> attachedPages)
        {
            Factories = new Dictionary<string, IPacketPage>();
            ReportData = reportData;

            foreach (var page in attachedPages)
            {

                // If FormId is not Guid means it is a policy template
                if (!GuidEx.IsGuid(page.FormId))
                {
                    var data = new ReportData(reportData.Risk) { 
                        Id = page.Id, 
                        Billing = reportData.Billing, 
                        Endorsement = reportData.Endorsement, 
                        DataSets = reportData.DataSets, 
                        AIWithNotice = reportData.AIWithNotice 
                    };

                    var factory = (IPacketPage)Activator.CreateInstance(Type.GetType($"{PACKETNAMESPACE}.{page.FormId}Page"), data);
                    Factories.Add($"{page.FormId}{page.Id}", factory);
                }
                else
                {
                    // User uploaded forms
                    var userPages = GetUserUploadedForms(page, riskDetailId);
                    var ctr = 0;
                    foreach (var userPage in userPages)
                    {
                        var test = userPage.ToString();
                        Factories.Add(string.Concat(userPage.ToString(), page.FormId, ++ctr), userPage);
                    }
                }
            }
            
        }

        private List<UserUploadPacketPage> GetUserUploadedForms(PageInfo page, Guid RiskDetailId)
        {
            var retList = new List<UserUploadPacketPage>();
            var forms = page.TemplateName.Split('|');
            foreach(var item in forms)
            {
                if (!string.IsNullOrEmpty(item))
                    retList.Add(new UserUploadPacketPage()
                    {
                        isAttached = true,
                        PageOrder = page.PageOrder,
                        TemplateName = item,
                        Id = Guid.Parse(page.FormId)
                    });
            }

            return retList;
        }

        private class GuidEx
        {
            public static bool IsGuid(string value)
            {
                Guid x;
                return Guid.TryParse(value, out x);
            }
        }

    }

    public enum PageList
    {
        AdditionalInsuredPage,
        IncidentalFarmingPage,
        OtherMembersOfHouseholdPage
    }
}
 