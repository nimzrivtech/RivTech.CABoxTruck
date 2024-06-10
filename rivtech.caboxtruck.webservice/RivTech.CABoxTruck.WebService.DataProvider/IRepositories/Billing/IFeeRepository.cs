using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing
{
    public interface IFeeRepository
    {
        /// <summary>
        /// Returns list of non-taxable FeeType based on given state.
        /// </summary>
        IEnumerable<FeeKind> GetNonTaxableTypesByState(string stateCode, DateTime effectiveDate);
        List<FeeDetails> GetFeeDetailsByState(string stateCode);
    }
}
