using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets
{
    public interface IDotInfoOptionsService
    {
        Task<DotInfoOptionsDto> GetAllAsync();
    }
}
