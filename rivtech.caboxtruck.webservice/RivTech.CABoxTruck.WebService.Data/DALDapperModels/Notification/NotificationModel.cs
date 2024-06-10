using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.DALDapperModels.Notification
{
    public class NotificationModel
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string NotificationDetails { get; set; }
        public int? CompletionPercentage { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string NotificationStatus { get; set; }
        public bool? IsRead { get; set; }
        public long? UserId { get; set; }
        public Guid? NotificationId { get; set; }
        public string PolicyNumber { get; set; }
        public string CompanyName { get; set; }
        public Guid? OldRiskDetailId { get; set; }
        public Guid? NewRiskDetailId { get; set; }
        public Guid? RiskId { get; set; }
        public bool IsRendered { get; set; }
        public string ErrorMessages { get; set; }
    }
}
