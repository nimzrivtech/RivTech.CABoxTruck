using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
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
    public class CovenirReportService : ICovenirReportService
    {
        private readonly IFTPDocumentFilesCountRepository _ftpDocumentFilesCountRepository;
        private readonly IMapper _mapper;

        public CovenirReportService(
            IFTPDocumentFilesCountRepository ftpDocumentFilesCountRepository,
            IMapper mapper
        )
        {
            _ftpDocumentFilesCountRepository = ftpDocumentFilesCountRepository;
            _mapper = mapper;
        }

        public async Task<List<CovenirReportDTO>> GetData(FTPDocumentFilesCountInputDTO request)
        {
            var filesCountDTOs = new List<FTPDocumentFilesCountDTO>();

            foreach (var day in EachDay(request.FromDate.Value, request.ToDate.Value))
            {
                var filesCounts = await _ftpDocumentFilesCountRepository.GetByDateAsync(day);

                if(filesCounts == null || filesCounts.Count == 0)
                {
                    continue;
                }
                else if (filesCounts.Count > 1)
                {
                    var regularCount = filesCounts.Sum(x => x.RegularCount);
                    var invoiceCount = filesCounts.Sum(x => x.InvoiceCount);
                    var certifiedCount = filesCounts.Sum(x => x.CertifiedCount);

                    filesCountDTOs.Add(new FTPDocumentFilesCountDTO
                    {
                        Id = filesCounts[0].Id,
                        RegularCount = regularCount,
                        InvoiceCount = invoiceCount,
                        CertifiedCount = certifiedCount,
                        FTPSentDate = day.Date,
                        CreatedBy = filesCounts[0].CreatedBy,
                        CreatedDate = filesCounts[0].CreatedDate
                    });
                }
                else
                {
                    filesCountDTOs.Add(_mapper.Map<FTPDocumentFilesCountDTO>(filesCounts[0]));
                }
            }

            return _mapper.Map<List<CovenirReportDTO>>(filesCountDTOs);
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
