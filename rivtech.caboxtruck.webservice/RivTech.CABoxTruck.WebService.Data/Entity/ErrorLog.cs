using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class ErrorLog : BaseEntity<Guid>
    {
        public long UserId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string JsonMessage { get; set; }
        public string Action { get; set; }
        public string Method { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
