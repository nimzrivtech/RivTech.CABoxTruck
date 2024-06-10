using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ILvDatasetService
    {
        Task<List<LvDatasetDTO>> GetByTypeAndRaterVersionAsync(string type,bool isCurrentlySupported);
        string GetByTypeAndRaterVersionAndValueAsync(string type, bool isCurrentlySupported,int Value);
    }
}
