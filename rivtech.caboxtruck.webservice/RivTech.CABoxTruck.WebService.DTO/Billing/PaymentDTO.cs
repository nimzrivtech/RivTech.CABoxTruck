using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }
        public Guid RiskId { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string InstrumentId { get; set; }
        public string Reference { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public Guid? VoidOfPaymentId { get; set; }
        public string VoidedById { get; set; }
        public DateTime? VoidDate { get; set; }

        //public EnumerationDTO InstrumentType { get; set; }
        public ApplicationUserDTO CreatedBy { get; set; }
        public ApplicationUserDTO VoidedBy { get; set; }
        //public List<PaymentDetailDTO> PaymentDetails { get; set; }

        public PayeeInfoDTO PayeeInfo { get; set; }

        public PaymentDTO()
        {
            //PaymentDetails = new List<PaymentDetailDTO>();
        }
    }
}
