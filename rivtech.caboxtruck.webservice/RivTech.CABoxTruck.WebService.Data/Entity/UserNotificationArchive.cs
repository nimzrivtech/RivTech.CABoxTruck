using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class UserNotificationArchive
    {
        [Key]
        public int Id { get; set; }
        public Guid UserNotificationArchiveId { get; set; }
        public Guid UserNotificationId { get; set; }
        public bool IsRead { get; set; }
        public long UserId { get; set; }
        public Guid NotificationId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateExpired { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
