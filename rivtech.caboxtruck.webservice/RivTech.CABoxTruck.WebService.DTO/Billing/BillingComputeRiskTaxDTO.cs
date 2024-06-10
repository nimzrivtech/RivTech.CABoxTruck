using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingComputeRiskTaxDTO
    {
        public DateTime EffectiveDate { get; set; }
        public List<TransactionDetailDTO> TransactionDetails { get; set; }
        public string StateCode { get; set; }
        public decimal? InstallmentCount { get; set; }
    }
}
