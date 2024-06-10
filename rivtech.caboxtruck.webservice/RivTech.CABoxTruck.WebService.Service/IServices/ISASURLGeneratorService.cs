using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ISASURLGeneratorService
    {
        Uri GenerateSharedAccessSignatureURL(string url);
        Uri GenerateSharedAccessSignatureURL(string url, string newFilename);
    }
}