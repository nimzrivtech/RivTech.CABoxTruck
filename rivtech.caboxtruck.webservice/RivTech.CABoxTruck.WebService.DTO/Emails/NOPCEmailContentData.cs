using System.IO;

namespace RivTech.CABoxTruck.WebService.DTO.Emails
{
    public class NOPCEmailContentData : EmailContentData
    {
        public NOPCEmailContentData(string toEmail, string fromEmail, string subject) 
            : base(toEmail, fromEmail, subject)
        {
        }

        public string PolicyNumber { get; set; }
        public string BusinessName { get; set; }

        public override string AttachmentFilename
        {
            get
            {
                var filename = $"Notice_of_Pending_Cancellation_{BusinessName.Replace(" ", "_")}_{PolicyNumber}";
                if (string.IsNullOrWhiteSpace(AttachmentPath))
                    return filename;
                var extension = Path.GetExtension(AttachmentPath);
                return $"{filename}{extension}".Trim(' ');
            }
        }

    }
}
