using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskStatusHistoryService
    {
        Task<bool> InsertAsync(RiskStatusHistory model);
    }
}
