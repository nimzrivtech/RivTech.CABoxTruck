using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class AuditLog : BaseEntity<long>, ICreatedData
    {
        public long UserId { get; set; }
        public string Description { get; set; }
        public string KeyID { get; set; }
        public string AuditType { get; set; }
        public string Action { get; set; }
        public string Method { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        // Added for Client Details
        public string IPAddress { get; set; }
        public string DeviceType { get; set; }
        public string OS { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string AppVersion { get; set; }
    }
}
