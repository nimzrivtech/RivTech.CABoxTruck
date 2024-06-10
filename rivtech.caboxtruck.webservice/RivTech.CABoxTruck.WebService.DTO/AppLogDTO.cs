using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO.Enum;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AppLogDTO
    {
        public AppLogDTO(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public long UserId { get; set; }
        public string TableName { get; set; }
        public string RequestValues { get; set; }
        public string ResponseValues { get; set; }
        public string Message { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AppLogType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public AppLog ToAudit()
        {
            var appLog = new AppLog();
            appLog.UserId = UserId;
            appLog.Type = AuditType.ToString();
            appLog.TableName = TableName;
            appLog.CreatedDate = DateTime.Now;
            appLog.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            appLog.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            appLog.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            appLog.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            return appLog;
        }
    }
}
