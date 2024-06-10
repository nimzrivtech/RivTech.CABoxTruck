using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class BlacklistedDriverDTO
    {
        public Guid? Id { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DriverLicenseNumber { get; set; }
        public bool IsActive { get; set; }
        public string Comments { get; set; }
    }
}
