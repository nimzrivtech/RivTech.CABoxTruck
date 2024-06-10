using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class FileUploadDocumentRepository : BaseRepository<FileUploadDocument, Guid>, IFileUploadDocumentRepository
    {
        public FileUploadDocumentRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<FileUploadDocument>> GetAllIncludeAsync()
        {
            return await _context.Set<FileUploadDocument>()
                                .Include(config => config.FileCategory)
                                .ToListAsync();
        }

        public async Task<IEnumerable<FileUploadDocument>> GetAllIncludeAsync(Expression<Func<FileUploadDocument, bool>> expression)
        {
            return await _context.Set<FileUploadDocument>()
                                .Where(expression)
                                .Include(config => config.FileCategory)
                                .Include(config => config.RiskDetail)
            .ToListAsync();
        }

        public async Task<(IEnumerable<FileUploadDocument>, PaginationDetails)> GetWithPagingAsync(Expression<Func<FileUploadDocument, bool>> expression, Expression<Func<FileUploadDocument, object>> selector, int pageNumber, int pageSize, string sorting)
        {
            try
            {
                var count = _context.Set<FileUploadDocument>().CountAsync(expression);

                int cnt = await count;
                IQueryable<FileUploadDocument> result;
                if (sorting == "ASC")
                {
                    result = _context.Set<FileUploadDocument>()
                                   .Where(expression).OrderBy(selector)
                                   .Include(config => config.FileCategory)
                                   .Include(config => config.RiskDetail).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    result = _context.Set<FileUploadDocument>()
                                  .Where(expression).OrderByDescending(selector)
                                  .Include(config => config.FileCategory)
                                  .Include(config => config.RiskDetail).Skip((pageNumber - 1) * pageSize).Take(pageSize);

                }
                

                var totalPages = (int)Math.Ceiling(cnt / (float)pageSize);
                
                return (await result.ToListAsync(), new PaginationDetails(pageNumber, totalPages, cnt));
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

    }
}


