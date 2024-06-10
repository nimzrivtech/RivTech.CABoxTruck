using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.Common.ReportTemplates;
using RivTech.CABoxTruck.WebService.DALDapper.IDataAccess;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Report.Handlers
{
    public class DMVReportHandler
    {
        private readonly IMapper _mapper;
        private readonly IReportDataAccess _reportDataAccess;
        private readonly IReportGenerator _reportGenerator;

        public DMVReportHandler(IMapper mapper, IReportDataAccess reportDataAccess, IReportGenerator reportGenerator)
    {
        _mapper = mapper;
        _reportDataAccess = reportDataAccess;
        _reportGenerator = reportGenerator;
    }

    public async Task<List<DMVReportTemplate>> Handler(ReportManagementFilter reportManagementFilter)
        {
            var dmvDataModel = await _reportDataAccess.GetDMVData(reportManagementFilter);
            return _mapper.Map<List<DMVReportTemplate>>(dmvDataModel);
        }

    }
}
