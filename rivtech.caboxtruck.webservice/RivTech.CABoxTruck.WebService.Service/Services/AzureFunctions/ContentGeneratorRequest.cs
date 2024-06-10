using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions
{
    public class ContentGeneratorRequest
    {
        public string Payload { get; set; }
        public string PayloadType { get; set; }
        public string TemplateName { get; set; }
        public string TemplateType { get; set; }
        public string DocumentType { get; set; }
    }
}
