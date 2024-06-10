using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Http.Features;
using Mono.TextTemplating;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
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
    public class DriverReportHandler
    {
        private readonly IMapper _mapper;
        private readonly IDriverService _driverService;

        public DriverReportHandler(IMapper mapper, IDriverService driverService)
        {
            _mapper = mapper;
            _driverService = driverService;
        }

        public async Task<List<DriverReportTemplate>> Handler(ReportManagementFilter reportManagementFilter)
        {
            var driverDataModels = await _driverService.GetByRiskDetailIdIncludeAsync(reportManagementFilter.RiskDetailId);

            //Return empty model template if no record found to proceed with the template download
            if (!driverDataModels.Any())
            {
                return new List<DriverReportTemplate>
                {
                    new DriverReportTemplate
                    {
                        FirstName = string.Empty,
                        MiddleName = string.Empty,
                        LastName = string.Empty,
                        LicenseNumber = string.Empty,
                        State = string.Empty,
                        BirthDate = string.Empty,
                        YrsCommDrivingExp = string.Empty
                    }
                };
            }
            else
            {
                //if(reportManagementFilter.SortField != null)
                //{
                //    PropertyInfo propertyInfo = typeof(DriverDTO).GetProperties()
                //        .FirstOrDefault(p => p.Name.Equals(reportManagementFilter.SortField, StringComparison.OrdinalIgnoreCase));

                //    if(propertyInfo != null)
                //    {
                //        ParameterExpression parameter = Expression.Parameter(typeof(DriverDTO));
                //        Expression propertyExpression = Expression.Property(parameter, propertyInfo);
                //        LambdaExpression lambdaExpression = Expression.Lambda(propertyExpression, parameter);
                //        MethodInfo orderByMethod = typeof(Queryable).GetMethods()
                //            .FirstOrDefault(method => method.Name == (reportManagementFilter.Ascending ? "OrderBy" : "OrderByDescending"))
                //            ?.MakeGenericMethod(typeof(DriverDTO), propertyInfo.PropertyType);
                //        IEnumerable<DriverDTO> sortedData = orderByMethod.Invoke(null, new object[] { driverDataModels.AsQueryable(), lambdaExpression }) as IEnumerable<DriverDTO>;

                //        return _mapper.Map<List<DriverReportTemplate>>(sortedData);
                //    }
                //}
            };

            return _mapper.Map<List<DriverReportTemplate>>(driverDataModels.OrderBy(o => o.CreatedDate));
        }
    }
}
