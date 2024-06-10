using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Dashboard;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Dashboard
{
    public interface IDashboardService
    {
        DashboardCountDto GetDashboardCount();
        Task<DashboardPolicySearchDTO> GetRiskByPolicyNumber(string policyNumber);
    }
}
