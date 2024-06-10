using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Emails
{
    public class EmailContentData
    {
        public EmailContentData(string toEmail, string fromEmail, string subject)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                throw new ArgumentException($"'{nameof(toEmail)}' cannot be null or whitespace.", nameof(toEmail));
            }

            if (string.IsNullOrWhiteSpace(fromEmail))
            {
                throw new ArgumentException($"'{nameof(fromEmail)}' cannot be null or whitespace.", nameof(fromEmail));
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException($"'{nameof(subject)}' cannot be null or whitespace.", nameof(subject));
            }

            ToEmail = toEmail;
            FromEmail = fromEmail;
            Subject = subject;
        }  
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string AttachmentPath { get; private set; }
        public virtual string AttachmentFilename { get; }

        public void AddAttachment(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }
            AttachmentPath = path;
        }
    }
}
