using System.Collections.Generic;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration
{
    public class ReportBuilder
    {
        private readonly ReportPayload _reportPayload;

        private ReportBuilder()
        {
            _reportPayload = new ReportPayload {FormsList = new List<Form>()};
        }

        public static ReportBuilder Init()
        {
            return new ReportBuilder();
        }

        public ReportBuilder AddPage(IPage page)
        {
            var pageData = page.GetData();
            _reportPayload.FormsList.Add(pageData);
            return this;
        }

        public ReportBuilder AddPage(IEnumerable<IPage> pages)
        {
            foreach (var page in pages)
            {
                AddPage(page);
            }
            return this;
        }

        public ReportBuilder SetUploadPath(string path)
        {
            _reportPayload.Upload = path;
            return this;
        }

        public ReportPayload Build()
        {
            return _reportPayload;
        }
    }
}
