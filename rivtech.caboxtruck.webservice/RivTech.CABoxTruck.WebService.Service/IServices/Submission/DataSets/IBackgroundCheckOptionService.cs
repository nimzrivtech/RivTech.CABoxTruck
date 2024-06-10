using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets
{
    public interface IBackgroundCheckOptionService
    {
        Task<List<EnumerationDTO>> GetAllAsync();
    }
}
