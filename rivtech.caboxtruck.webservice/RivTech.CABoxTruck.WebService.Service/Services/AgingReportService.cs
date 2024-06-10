using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.Common.ReportTemplates;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public interface IAgingReportService
    {
        Task<List<AgingReportTemplate>> GetData(AgingReportRequestDTO request);
        Task<Stream> GetReportStream(AgingReportRequestDTO request);
        Task<List<AgingReportDTO>> GetRCAging(AgingReportRequestDTO request);
        Task<List<AgingReportDTO>> GetCAging(string ReportType, AgingReportRequestDTO request);
    }

    public class AgingReportService : IAgingReportService
    {
        private readonly IMapper _mapper;
        private readonly IAgingReportRepository _agingReportRepository;
        private readonly IReportGenerator _reportGenerator;

        public AgingReportService(
            IMapper mapper,
            IAgingReportRepository agingReportRepository,
            IReportGenerator reportGenerator
        )
        {
            this._mapper = mapper;
            this._agingReportRepository = agingReportRepository;
            this._reportGenerator = reportGenerator;
        }

        public async Task<Stream> GetReportStream(AgingReportRequestDTO request)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                var reportContent = string.Empty;
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    FloatParseHandling = FloatParseHandling.Decimal
                };

                var templates = await this.GetData(request);
                reportContent = JsonConvert.SerializeObject(templates, settings);

                _reportGenerator.GenerateReportToExcel("AgingReport", reportContent, stream);
            }
            catch (Exception)
            {
                throw;
            }

            return stream;
        }

        public async Task<List<AgingReportTemplate>> GetData(AgingReportRequestDTO request)
        {
            var data = await this._agingReportRepository.GetAsync(request);
            return this._mapper.Map<List<AgingReportTemplate>>(data);
        }

        public async Task<List<AgingReportDTO>> GetRCAging(AgingReportRequestDTO request)
        {
            var data = await this._agingReportRepository.GetAsync(request);
            return data.ToList();
        }
        public async Task<List<AgingReportDTO>> GetCAging(string ReportType, AgingReportRequestDTO request)
        {
            var data = await this._agingReportRepository.GetAgingAsync(ReportType,request);
            return data.ToList();
        }
    }
}
