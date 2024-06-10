using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class FTPDocumentFilesCountDTO
    {
        public Guid? Id { get; set; }
        public int? RegularCount { get; set; }
        public int? InvoiceCount { get; set; }
        public int? CertifiedCount { get; set; }
        public DateTime? FTPSentDate { get; set; } 
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class FTPDocumentFilesCountInputDTO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
