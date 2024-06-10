using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public interface IPremiumEarnedTaxReportService
    {
        Task<List<PremiumEarnedTaxReportDTO>> GetData(PremiumEarnedTaxReportRequestDTO request);
    }

    public class PremiumEarnedTaxReportService : IPremiumEarnedTaxReportService
    {
        private readonly IPremiumEarnedTaxReportRepository _premiumEarnedTaxReportRepository;

        public PremiumEarnedTaxReportService(
            IPremiumEarnedTaxReportRepository premiumEarnedTaxReportRepository
        )
        {
            _premiumEarnedTaxReportRepository = premiumEarnedTaxReportRepository;
        }

        public async Task<List<PremiumEarnedTaxReportDTO>> GetData(PremiumEarnedTaxReportRequestDTO request)
        {
            var data = await _premiumEarnedTaxReportRepository.GetAsync(request);
            return data.ToList();
        }
    }
}
