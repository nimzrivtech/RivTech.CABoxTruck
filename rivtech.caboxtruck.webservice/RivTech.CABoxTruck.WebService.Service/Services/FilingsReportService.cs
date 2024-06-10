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
    public class FilingsReportService : IFilingsReportService
    {
        private readonly IFilingsReportRepository _filingsReportRepository;

        public FilingsReportService(
            IFilingsReportRepository filingsReportRepository
        )
        {
            _filingsReportRepository = filingsReportRepository;
        }

        public async Task<List<FilingsReportDTO>> GetData()
        {
            var data = await _filingsReportRepository.GetAsync();
            return data.ToList();
        }
    }
}
