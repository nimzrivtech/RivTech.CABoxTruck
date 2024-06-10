using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.DocumentGeneration
{
    public class ReportPayload
    {
        public string AuthorName { get; set; }
        public string Keywords { get; set; }
        public string Upload { get; set; }
        public List<Form> FormsList { get; set; }
    }

    public class Form
    {
        public string TemplateName { get; set; }
        public int PageOrder { get; set; }
        public Dictionary<string, string> FormValues { get; set; }
        public Dictionary<string, List<Dictionary<string, string>>> TableValues { get; set; }
    }
}
