using AutoMapper;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.Common.ReportTemplates;
using RivTech.CABoxTruck.WebService.DALDapper.IDataAccess;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Report.Handlers
{
    public class POMReportHandler
    {
        private readonly IMapper _mapper;
        private readonly IReportDataAccess _reportDataAccess;
        private readonly IReportGenerator _reportGenerator;

        public POMReportHandler(IMapper mapper, IReportDataAccess reportDataAccess, IReportGenerator reportGenerator)
        {
            _mapper = mapper;
            _reportDataAccess = reportDataAccess;
            _reportGenerator = reportGenerator;
        }

        public async Task<List<POMReportTemplate>> Handler(ReportManagementFilter reportManagementFilter)
        {
            var pomDataModel = await _reportDataAccess.GetPOMData(reportManagementFilter);
            return _mapper.Map<List<POMReportTemplate>>(pomDataModel);
        }
    }
}
