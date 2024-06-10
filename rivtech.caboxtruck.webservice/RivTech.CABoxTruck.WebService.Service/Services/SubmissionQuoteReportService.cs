using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
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
    public class SubmissionQuoteReportService : ISubmissionQuoteReportService
    {
        private readonly ISubmissionQuoteReportRepository _submissionQuoteReportRepository;

        public SubmissionQuoteReportService(
            ISubmissionQuoteReportRepository submissionQuoteReportRepository
        )
        {
            _submissionQuoteReportRepository = submissionQuoteReportRepository;
        }

        public async Task<List<SubmissionQuoteReportDTO>> GetData(SubmissionQuoteReportRequestDTO request)
        {
            var data = await _submissionQuoteReportRepository.GetAsync(request);
            return data.ToList();
        }
    }
}
