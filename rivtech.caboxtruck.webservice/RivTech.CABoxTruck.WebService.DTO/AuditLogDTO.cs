using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AuditLogDTO
    {
        public Int64 AuditLogID { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public Int64 Key { get; set; }
        public string AuditType { get; set; }
        public string Action { get; set; }
        public string Method { get; set; }
    }
}