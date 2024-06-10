using System.Collections.Generic;
using RivTech.CABoxTruck.WebService.DTO.Dashboard;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IDashboardRepository
    {
        IEnumerable<dynamic> GetDashboardCount();
    }
}
