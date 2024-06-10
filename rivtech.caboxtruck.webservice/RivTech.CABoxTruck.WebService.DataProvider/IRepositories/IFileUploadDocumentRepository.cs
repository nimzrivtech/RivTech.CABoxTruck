using AutoMapper.Execution;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IFileUploadDocumentRepository : IRepository<FileUploadDocument, Guid>
    {
        Task<IEnumerable<FileUploadDocument>> GetAllIncludeAsync();
        Task<IEnumerable<FileUploadDocument>> GetAllIncludeAsync(Expression<Func<FileUploadDocument, bool>> expression);
        Task<(IEnumerable<FileUploadDocument>, PaginationDetails)> GetWithPagingAsync(Expression<Func<FileUploadDocument, bool>> expression, Expression<Func<FileUploadDocument, object>> selector, int pageNumber, int pageSize, string sorting);
        
    }

   
}
