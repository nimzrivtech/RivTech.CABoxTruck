using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public Guid NotificationId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string NotificationDetails { get; set; }
        public int? CompletionPercentage { get; set; }
        public long? UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateExpired { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string NotificationStatus { get; set; }
        public string PolicyNumber { get; set; }
        public string CompanyName { get; set; }
        public Guid? RiskId { get; set; }
        public Guid? OldRiskDetailId { get; set; }
        public Guid? NewRiskDetailId { get; set; }
        public bool IsRendered { get; set; }
        public string ErrorMessages { get; set; }
    }
}
