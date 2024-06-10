using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class FTPDocumentRepository : BaseRepository<FTPDocument, Guid>, IFTPDocumentRepository
    {
        public FTPDocumentRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<bool> UpdateByBatchId(List<FTPDocumentTemporary> collectionData)
        {
            var tempData = collectionData.GroupBy(x => x.BatchId).ToList();
            foreach (var data in tempData)
            {
                var ftpDocumentData = _context.FTPDocument.Where(x => x.BatchId == data.Select(x => x.BatchId).FirstOrDefault().ToString());
                foreach (var ftptempData in ftpDocumentData)
                {
                    ftptempData.IsCompiled = true;
                    ftptempData.IsActive = true;
                    ftptempData.IsUploaded = true;
                    ftptempData.UpdatedDate = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}


