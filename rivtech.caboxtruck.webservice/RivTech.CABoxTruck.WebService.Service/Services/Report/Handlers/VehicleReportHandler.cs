using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using Mono.TextTemplating;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.Common.ReportTemplates;
using RivTech.CABoxTruck.WebService.DALDapper.IDataAccess;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Migrations;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Report.Handlers
{
    public class VehicleReportHandler
    {
        private readonly IMapper _mapper;
        private readonly IVehicleService _vehicleService;
        private readonly IListValueTextParser _listValueTextParser;
        private readonly IRiskService _riskService;
        private readonly ILimitsService _limitsService;

        public VehicleReportHandler(IMapper mapper, IVehicleService vehicleService, IListValueTextParser listValueTextParser,
            IRiskService riskService, ILimitsService limitsService)
        {
            _mapper = mapper;
            _vehicleService = vehicleService;
            _listValueTextParser = listValueTextParser;
            _riskService = riskService;
            _limitsService = limitsService;
        }

        public async Task<List<VehicleReportTemplate>> Handler(ReportManagementFilter reportManagementFilter)
        {
            var vehicleDataModels = await _vehicleService.GetByRiskDetailIdIncludeAsync(reportManagementFilter.RiskDetailId);

            //Return empty model template if no record found to proceed with the template download
            if (!vehicleDataModels.Any())
            {
                return new List<VehicleReportTemplate>
                {
                    new VehicleReportTemplate
                    {
                        VIN = string.Empty,
                        Year = string.Empty,
                        Make = string.Empty,
                        Model = string.Empty,
                        RegisteredState = string.Empty,
                        StatedAmount = string.Empty,
                        VehicleType = string.Empty,
                        Description = string.Empty,
                        BusinessClass = string.Empty,
                        GrossVehicleWeight = string.Empty,
                        Radius = string.Empty,
                        IsCoverageFireTheft = string.Empty,
                        IsCoverageCollision = string.Empty,
                        CoverageFireTheft = string.Empty,
                        CoverageCollisionDeductible = string.Empty,
                        HasCoverageCargo = string.Empty,
                        HasCoverageRefrigeration = string.Empty
                    }
                };
            }

            var riskDto = await _riskService.GetByIdIncludeAsync(reportManagementFilter.RiskDetailId);
            var limitsDataSets = await _limitsService.GetPerStateAsync(riskDto.NameAndAddress?.State, riskDto.RaterVersionId);

            foreach (var vehicleDataModel in vehicleDataModels)
            {
                vehicleDataModel.Description = await _listValueTextParser.ParseAsync(vehicleDataModel.VehicleDescriptionId, ListParserType.VehicleDescription, riskDto.RaterVersionId);
                vehicleDataModel.BusinessClass = await _listValueTextParser.ParseAsync(vehicleDataModel.VehicleBusinessClassId, ListParserType.AccountCategory);
                vehicleDataModel.GrossVehicleWeight = await _listValueTextParser.ParseAsync(vehicleDataModel.GrossVehicleWeightId, ListParserType.MainGVWInLbs);

                vehicleDataModel.CoverageFireTheft = limitsDataSets.ComprehensiveLimits.Find(c => c.Id == vehicleDataModel.CoverageFireTheftDeductibleId)?.LimitDisplay;
                vehicleDataModel.CoverageCollisionDeductible = limitsDataSets.CollisionLimits.Find(c => c.Id == vehicleDataModel.CoverageCollisionDeductibleId)?.LimitDisplay;
            }

            //if (reportManagementFilter.SortField != null)
            //{
            //    if (reportManagementFilter.SortField.Equals("vehicledescription", StringComparison.CurrentCultureIgnoreCase))
            //        reportManagementFilter.SortField = "Description";
            //    if (reportManagementFilter.SortField.Equals("state", StringComparison.CurrentCultureIgnoreCase))
            //        reportManagementFilter.SortField = "RegisteredState";

            //    PropertyInfo propertyInfo = typeof(VehicleDTO).GetProperties()
            //        .FirstOrDefault(p => p.Name.Equals(reportManagementFilter.SortField, StringComparison.OrdinalIgnoreCase));

            //    if (propertyInfo != null)
            //    {
            //        ParameterExpression parameter = Expression.Parameter(typeof(VehicleDTO));
            //        Expression propertyExpression = Expression.Property(parameter, propertyInfo);
            //        LambdaExpression lambdaExpression = Expression.Lambda(propertyExpression, parameter);
            //        MethodInfo orderByMethod = typeof(Queryable).GetMethods()
            //            .FirstOrDefault(method => method.Name == (reportManagementFilter.Ascending ? "OrderBy" : "OrderByDescending"))
            //            ?.MakeGenericMethod(typeof(VehicleDTO), propertyInfo.PropertyType);
            //        IEnumerable<VehicleDTO> sortedData = orderByMethod.Invoke(null, new object[] { vehicleDataModels.AsQueryable(), lambdaExpression }) as IEnumerable<VehicleDTO>;

            //        return _mapper.Map<List<VehicleReportTemplate>>(sortedData);
            //    }
            //}

            return _mapper.Map<List<VehicleReportTemplate>>(vehicleDataModels);
        }
    }
}