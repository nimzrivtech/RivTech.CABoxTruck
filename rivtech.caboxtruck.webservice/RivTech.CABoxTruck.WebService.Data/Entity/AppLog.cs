using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class AppLog : BaseEntity<long>
    {
        public long UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
        public string RequestValues { get; set; }
        public string ResponseValues { get; set; }
        public string Message { get; set; }
    }
}
