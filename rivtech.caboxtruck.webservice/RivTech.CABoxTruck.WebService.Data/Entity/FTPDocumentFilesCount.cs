using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class FTPDocumentFilesCount : BaseEntity<Guid>, ICreatedData
    {
        public FTPDocumentFilesCount()
        {
            FTPSentDate = DateTime.Now.Date;
        }
        public int? RegularCount { get; set; }
        public int? InvoiceCount { get; set; }
        public int? CertifiedCount { get; set; }
        public DateTime? FTPSentDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
    }
}
