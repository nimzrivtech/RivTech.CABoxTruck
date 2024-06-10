using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IFTPDocumentRepository : IRepository<FTPDocument, Guid>
    {
        Task<bool> UpdateByBatchId(List<FTPDocumentTemporary> collectionData);
    }
}
