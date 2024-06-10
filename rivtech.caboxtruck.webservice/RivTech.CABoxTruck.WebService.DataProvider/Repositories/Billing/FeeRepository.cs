using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Billing
{
    public class FeeRepository : IFeeRepository
    {
        private readonly AppDbContext _dbContext;

        public FeeRepository(
            AppDbContext dbContext
        ) {
            _dbContext = dbContext;
        }

        public IEnumerable<FeeKind> GetNonTaxableTypesByState(string stateCode, DateTime effectiveDate)
        {
            var fees = _dbContext.NonTaxableFees
                .Where(ntf => ntf.StateCode.Equals(stateCode) 
                    && effectiveDate.Date >= ntf.EffectiveDate.Date);
            return fees.Select(ntf => FeeKind.FromId(ntf.FeeKindId)).ToList();

        }

        public List<FeeDetails> GetFeeDetailsByState(string stateCode)
        {
            var feeDetails = _dbContext.FeeDetails.Where(fd => fd.StateCode == stateCode && fd.IsActive);
            return feeDetails.Select(fd => new FeeDetails(fd.StateCode, fd.FeeKindId, fd.Description, fd.Amount, fd.IsActive)).ToList();
        }
    }
}
