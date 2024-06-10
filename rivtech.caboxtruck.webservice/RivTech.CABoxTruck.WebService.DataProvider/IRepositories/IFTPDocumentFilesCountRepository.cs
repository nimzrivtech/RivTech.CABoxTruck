using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IFTPDocumentFilesCountRepository : IRepository<FTPDocumentFilesCount, Guid>
    {
        Task<List<FTPDocumentFilesCount>> GetByDateAsync(DateTime date);
        Task<List<FTPDocumentFilesCount>> GetByDateRangeRequestAsync(FTPDocumentFilesCountInputDTO request);
    }
}
