using System;
using System.Runtime.Serialization;

namespace RivTech.CABoxTruck.WebService.DTO.User
{
    public class UserInfoDTO
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid? SubAgencyId { get; set; }
        public Guid? AgentId { get; set; }
        public bool IsLoggedIn { get; set; }

        [IgnoreDataMember]
        public bool IsActive => true;
    }
}
