using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlTypes;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class AccountsReceivableRepository : IAccountsReceivableRepository
    {
        private readonly AppDbContext _context;
        public AccountsReceivableRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountsReceivableDTO>> SearchAcountsRecievableAsync(AccountsReceivableInputDTO data)
        {
            using (SqlConnection conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                await conn.OpenAsync();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@sortNameBy", data.SortNameBy);
                    param.Add("@receivableType", data.ReceivableType);
                    param.Add("@broker", data.Broker);
                    param.Add("@pastDueOnly", data.PastDueOnly);
                    param.Add("@cancellationStatus", data.CancellationStatus);
                    param.Add("@noPaymentsMade", data.NoPaymentsMade);
                    param.Add("@valuationDate", data.ValuationDate);
                    var result = await conn.QueryAsync<AccountsReceivableDTO>("SearchAccountsReceivable @sortNameBy, @receivableType, @broker, @pastDueOnly, @cancellationStatus, @noPaymentsMade, @valuationDate",
                                                                            param, commandType: CommandType.Text, commandTimeout: 300
                                                                            );
                    return result.ToList();
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }

        
    }
}


