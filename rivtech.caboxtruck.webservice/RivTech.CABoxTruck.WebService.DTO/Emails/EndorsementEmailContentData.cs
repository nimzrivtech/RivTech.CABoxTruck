using System.IO;

namespace RivTech.CABoxTruck.WebService.DTO.Emails
{
    public class EndorsementEmailContentData : EmailContentData
    {
        public EndorsementEmailContentData(string toEmail, string fromEmail, string subject) 
            : base(toEmail, fromEmail, subject)
        {
        }

        public string PolicyNumber { get; set; }
        public string BusinessName { get; set; }

        public override string AttachmentFilename
        {
            get
            {
                var filename = $"Endorsement_{BusinessName.Replace(" ", "_")}_{PolicyNumber}";
                if (string.IsNullOrWhiteSpace(AttachmentPath))
                    return filename.TrimEnd('"');
                var extension = Path.GetExtension(AttachmentPath);
                return $"{filename}{extension}".Trim(' ').TrimEnd('"');
            }
        }

    }
}
