using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO
{
    public class GenerateReportResponse<T>
    {
        public T Output { get; set; }

        public List<string> Errors { get; set; }
    }
}
