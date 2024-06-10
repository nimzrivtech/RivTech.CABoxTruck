using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class PaymentPostingDTO
    {
        public Guid? Id { get; set; }
        public short CreditedOfficeId { get; set; }
        public string PolicyNumber { get; set; }
        public string CompanyName { get; set; }
        public string Broker { get; set; }
        public string CreditedOffice { get; set; }
        public string CheckNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Decimal? PaymentAmount { get; set; }
        public Decimal? PremiumAmountPD { get; set; }
        public Decimal? PremiumAmountAL { get; set; }
        public Decimal? Premium { get; set; }
        public Decimal? Tax { get; set; }
        public Decimal? NonPremium { get; set; }
        public Decimal? OverPayment { get; set; }
        public Decimal? BrokerCommisionAL { get; set; }
        public Decimal? BrokerCommisionPD { get; set; }
        public Decimal? CommisionALAmount { get; set; }
        public Decimal? CommisionPDAmount { get; set; }
        public Decimal? CommissionTotal { get; set; }
        public Decimal? PercentageARS { get; set; }
        public Decimal? CommissionARS { get; set; }
        public Decimal? CommisionRivAL { get => ((22.7m - BrokerCommisionAL) * (PremiumAmountAL?? 0)/100); set { } }
        public Decimal? CommisionRivPD { get => ((10m / 100m) * (PremiumAmountPD?? 0)); set { } }
        public Decimal? CommisionToRivAmnt { get => (CommisionRivAL + CommisionRivPD - CommissionARS); set { } }
        public Decimal? PremiumDueToCarier { get => (Premium - CommissionTotal - CommisionToRivAmnt); set { } }
        public short PaymentTypesId { get; set; }
    }

    public class PaymentPostingInputDTO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
