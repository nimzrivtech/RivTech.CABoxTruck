using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class BorderauxReportService : IBorderauxReportService
    {
        private readonly IBorderauxReportRepository _borderauxReportRepository;

        public BorderauxReportService(
            IBorderauxReportRepository borderauxReportRepository
        )
        {
            _borderauxReportRepository = borderauxReportRepository;
        }

        public async Task<List<BorderauxReportDTO>> GetData(BorderauxReportRequestDTO request)
        {
            var data = await _borderauxReportRepository.GetAsync(request);
            return data.ToList();
        }
    }
}
