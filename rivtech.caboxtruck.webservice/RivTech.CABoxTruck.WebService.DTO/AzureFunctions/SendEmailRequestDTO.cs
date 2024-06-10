namespace RivTech.CABoxTruck.WebService.DTO.AzureFunctions
{
    public class SendEmailRequestDTO
    {
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string BccAddress { get; set; }
        public string CcAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string AttachmentUrl { get; set; }
        public string AttachmentFileName { get; set; }
    }
}
