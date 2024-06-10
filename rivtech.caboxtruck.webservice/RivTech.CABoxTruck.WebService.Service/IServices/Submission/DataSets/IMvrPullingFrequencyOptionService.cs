using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets
{
    public interface IMvrPullingFrequencyOptionService
    {
        Task<List<EnumerationDTO>> GetAllAsync();
    }
}
