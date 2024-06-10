using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RivTech.CABoxTruck.WebService.ExternalData.Entity
{
    public class EmailQueue : BaseEntity<Guid>
    {
        public string Address { get; set; }
        public string BccAddress { get; set; }
        public string CcAddress { get; set; }
        public string FromAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentUrl { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Type { get; set; }
        public bool IsSent { get; set; }
    }
}
