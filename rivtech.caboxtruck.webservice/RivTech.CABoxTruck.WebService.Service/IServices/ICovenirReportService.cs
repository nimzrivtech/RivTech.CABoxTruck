using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ICovenirReportService
    {
        Task<List<CovenirReportDTO>> GetData(FTPDocumentFilesCountInputDTO request);
    }
}
