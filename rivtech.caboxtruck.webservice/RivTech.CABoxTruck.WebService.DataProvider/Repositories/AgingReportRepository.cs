using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public interface IAgingReportRepository
    {
        Task<IEnumerable<AgingReportDTO>> GetAsync(AgingReportRequestDTO request);
        Task<IEnumerable<AgingReportDTO>> GetAgingAsync(string ReportType, AgingReportRequestDTO request);

    }

    public class AgingReportRepository: IAgingReportRepository
    {
        private readonly AppDbContext _context;

        public AgingReportRepository(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<AgingReportDTO>> GetAsync(AgingReportRequestDTO request)
        {
            using var conn = new SqlConnection(this._context.Database.GetDbConnection().ConnectionString);
            await conn.OpenAsync();
            try
            {
                request.ToDate = request.ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                var param = new DynamicParameters();
                param.Add("@toDate", request.ToDate);

                var result = await conn.QueryAsync<AgingReportDTO>(
                "GenerateAgingReport @toDate",
                    param,
                    commandType: CommandType.Text,
                    commandTimeout: 300);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        public async Task<IEnumerable<AgingReportDTO>> GetAgingAsync(string ReportType,AgingReportRequestDTO request)
        {
            using var conn = new SqlConnection(this._context.Database.GetDbConnection().ConnectionString);
            await conn.OpenAsync();
            try
            {
                request.ToDate = request.ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                var param = new DynamicParameters();
                param.Add("@toDate", request.ToDate);

                var rawResult = await conn.QueryAsync<AgingReportDTO>(
                $"{ReportType} @toDate",
                    param,
                    commandType: CommandType.Text,
                    commandTimeout: 420);

               

                return rawResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
       
    }
}
