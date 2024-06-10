using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.DALDapper.IDataAccess;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.Report;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using RivTech.CABoxTruck.WebService.Service.Services.Report.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly IReportDataAccess _reportDataAccess;
        private readonly IReportGenerator _reportGenerator;
        private readonly IBillingService _billingService;
        private readonly IVehicleService _vehicleService;
        private readonly IDriverService _driverService;
        private readonly IListValueTextParser _listValueTextParser;
        private readonly IRiskService _riskService;
        private readonly ILimitsService _limitsService;

        public ReportService(IMapper mapper, IReportDataAccess reportDataAccess, IReportGenerator reportGenerator, IBillingService billingService, 
            IVehicleService vehicleService, IListValueTextParser listValueTextParser, IRiskService riskService, ILimitsService limitsService,
            IDriverService driverService)
        {
            _mapper = mapper;
            _reportDataAccess = reportDataAccess;
            _reportGenerator = reportGenerator;
            _billingService = billingService;
            _vehicleService = vehicleService;
            _listValueTextParser = listValueTextParser;
            _riskService = riskService;
            _limitsService = limitsService;
            _driverService = driverService;
        }
        public async Task<Stream> GetReportStream(ReportManagementFilter reportManagementFilter)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                string reportContent = string.Empty;
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    FloatParseHandling = FloatParseHandling.Decimal
                };

                switch (reportManagementFilter.ReportType)
                {
                    case "DMVReport":
                        DMVReportHandler dMVReportHandler = new DMVReportHandler(_mapper, _reportDataAccess, _reportGenerator);
                        var dMVReportTemplates = await dMVReportHandler.Handler(reportManagementFilter);
                        reportContent = JsonConvert.SerializeObject(dMVReportTemplates, settings);
                        break;
                    case "POMReport":
                        POMReportHandler pomReportHandler = new POMReportHandler(_mapper, _reportDataAccess, _reportGenerator);
                        var pomReportTemplates = await pomReportHandler.Handler(reportManagementFilter);
                        reportContent = JsonConvert.SerializeObject(pomReportTemplates, settings);
                        break;
                    case "InvoiceDetailReport":
                        InvoiceDetailReportHandler invoiceDetailReportHandler = new InvoiceDetailReportHandler(_mapper, _billingService);
                        var invoiceDetailReportTemplates = await invoiceDetailReportHandler.Handler(reportManagementFilter);
                        reportContent = JsonConvert.SerializeObject(invoiceDetailReportTemplates, settings);
                        break;
                    case "VehicleReport":
                        VehicleReportHandler vehicleReportHandler = new VehicleReportHandler(_mapper, _vehicleService, _listValueTextParser, _riskService, _limitsService);
                        var vehicleReportTemplates = await vehicleReportHandler.Handler(reportManagementFilter);
                        reportContent = JsonConvert.SerializeObject(vehicleReportTemplates, settings);
                        break;
                    case "DriverReport":
                        DriverReportHandler driverReportHandler = new DriverReportHandler(_mapper, _driverService);
                        var driverReportTemplates = await driverReportHandler.Handler(reportManagementFilter);
                        reportContent = JsonConvert.SerializeObject(driverReportTemplates, settings);
                        break;
                }
                _reportGenerator.GenerateReportToExcel(reportManagementFilter.ReportType, reportContent, memoryStream, reportManagementFilter.isRaterV7);
            }
            catch (Exception e)
            {
            }

            return memoryStream;

        }

        public async Task<List<RiskDTO>> GetAllRiskDetails()
        {
            var riskDetailModel = _reportDataAccess.GetAllRiskDetails();
            return _mapper.Map<List<RiskDTO>>(riskDetailModel);
        }
    }
}
