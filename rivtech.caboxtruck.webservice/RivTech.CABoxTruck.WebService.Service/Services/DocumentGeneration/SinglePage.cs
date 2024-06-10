using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration
{
    public abstract class SinglePage
    {
        protected SinglePage(ReportData reportData)
        {
            ReportData = reportData;
        }

        public ReportData ReportData { get; }
    }
}
