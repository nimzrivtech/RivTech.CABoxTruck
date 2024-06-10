using System;

namespace RivTech.CABoxTruck.WebService.DTO.Report
{
    public class CovenirReportDTO
    {
        public int? RegularCount { get; set; }
        public int? InvoiceCount { get; set; }
        public int? CertifiedCount { get; set; }
        public DateTime? FTPSentDate { get; set; }
    }
}
