using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IFTPClientServerSevice
    {
        Task<bool> SendZipToFTPServerAsync(string batchId = null);
        Task SaveFTPDocument(Guid riskId, FileUploadDocument fileUploadDocument);
    }
}
