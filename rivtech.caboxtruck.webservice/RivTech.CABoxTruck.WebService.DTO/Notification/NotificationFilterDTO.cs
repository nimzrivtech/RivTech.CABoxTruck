using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Notification
{
    public class NotificationFilterDTO : FilterBase
    {
        public long? UserId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string NotificationDetails { get; set; }
        public string NotificationStatus { get; set; }
        public int? CompletionPercentage { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Order { get; set; }
        public bool IsRead { get; set; }
        public string PolicyNumber { get; set; }
        public string CompanyName { get; set; }
        public Guid? OldRiskDetailId { get; set; }
        public Guid? NewRiskDetailId { get; set; }
        public Guid? RiskId { get; set; }
        public Guid? NotificationId { get; set; }
        public bool IsRendered { get; set; }
        public string ErrorMessages { get; set; }

    }
}
