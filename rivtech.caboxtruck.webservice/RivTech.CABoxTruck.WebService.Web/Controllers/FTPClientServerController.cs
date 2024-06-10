using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FTPClientServerController : ControllerBase
    {
        private readonly IFTPClientServerSevice _IFTPClientServer;

        public FTPClientServerController(IFTPClientServerSevice IFTPClientServer)
        {
            _IFTPClientServer = IFTPClientServer;
        }

        [HttpPost("ftpZipSend")]
        [Produces("application/json")]
        public async Task<bool> SendZipToFTPServerAsync(string batchId)
        {
            await _IFTPClientServer.SendZipToFTPServerAsync(batchId);

            return true;
        }

    }
}
