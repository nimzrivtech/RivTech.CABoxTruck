using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using System;

namespace RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration
{
    public interface IPacketPage : IPage
    {
        public bool isAttached { get; }
    }

    public class UserUploadPacketPage : IPacketPage
    {
        public bool isAttached { get; set; }
        public string TemplateName { get; set; }
        public int PageOrder { get; set; }
        public Guid Id { get; set; }
        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"risk-document/user-upload/{Id}/{TemplateName}",
            };
            return formData;
        }
    }
}
