using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class BrokerCommissionDTO
    {
        public Guid? Id { get; set; }
        public string CreditedOffice { get; set; }
        public string PolicyNumber { get; set; }
        public string Broker { get; set; }
        public string CompanyName { get; set; }
        public string CheckNumber { get; set; }
        public Decimal? PaymentAmount { get; set; }
        public DateTime? VoidDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Decimal? BrokerCommisionAL { get; set; }
        public Decimal? BrokerCommisionPD { get; set; }
        public Decimal? CommisionALAmount { get; set; }
        public Decimal? CommisionPDAmount { get; set; }
        public Decimal? CommissionTotal { get => (CommisionALAmount ?? 0) + (CommisionPDAmount ?? 0); set { } }
        public Decimal? PercentageARS { get; set; }
        public Decimal? CommissionARS { get; set; }
    }

    public class BrokerCommissionInputDTO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
