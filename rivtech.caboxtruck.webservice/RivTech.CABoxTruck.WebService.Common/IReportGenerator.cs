using System.IO;

namespace RivTech.CABoxTruck.WebService.Common
{
    public interface IReportGenerator
    {
        void GenerateReportToExcel(string reportType, string reportContent, MemoryStream memoryStream, bool? isRaterV7 = null);
    }
}
