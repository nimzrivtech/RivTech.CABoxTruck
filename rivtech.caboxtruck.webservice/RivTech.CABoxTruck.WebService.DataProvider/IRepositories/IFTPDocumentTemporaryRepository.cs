using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IFTPDocumentTemporaryRepository : IRepository<FTPDocumentTemporary, Guid>
    {
        Task<bool> UpdateByBatchId(List<FTPDocumentTemporary> collectionData);
    }
}
