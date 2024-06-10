using System.IO;

namespace RivTech.CABoxTruck.WebService.DTO.Emails.Invoices
{
    public class AgentInvoiceEmailContentData : EmailContentData
    {
        public AgentInvoiceEmailContentData(string toEmail, string fromEmail, string subject) 
            : base(toEmail, fromEmail, subject)
        {
        }

        public string PolicyNumber { get; set; }
        public string BusinessName { get; set; }
        public string InvoiceNumber { get; set; }

        public override string AttachmentFilename
        {
            get
            {
                var filename = $"Policy_Invoice_{BusinessName.Replace(" ", "_")}_{InvoiceNumber}";
                if (string.IsNullOrWhiteSpace(AttachmentPath))
                    return filename;
                var extension = Path.GetExtension(AttachmentPath);
                return $"{filename}{extension}".Trim(' ');
            }
        }

    }
}
