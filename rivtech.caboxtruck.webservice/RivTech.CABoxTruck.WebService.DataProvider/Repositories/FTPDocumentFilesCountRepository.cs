using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class FTPDocumentFilesCountRepository : BaseRepository<FTPDocumentFilesCount, Guid>, IFTPDocumentFilesCountRepository
    {
        public FTPDocumentFilesCountRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<FTPDocumentFilesCount>> GetByDateAsync(DateTime date)
        {
            return await _context.FTPDocumentFilesCount
                           .Where(x => x.FTPSentDate == date)
                           .OrderBy(x => x.FTPSentDate)
                           .ToListAsync();
        }

        public async Task<List<FTPDocumentFilesCount>> GetByDateRangeRequestAsync(FTPDocumentFilesCountInputDTO request)
        {
            return await _context.FTPDocumentFilesCount
                           .Where(x => x.FTPSentDate >= request.FromDate && x.FTPSentDate <= request.ToDate)
                           .OrderBy(x => x.FTPSentDate)
                           .ToListAsync();
        }
    }
}
