using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets
{
    public interface IDriverInfoOptionsService
    {
        Task<DriverInfoOptionsDto> GetAllAsync();
    }
}
