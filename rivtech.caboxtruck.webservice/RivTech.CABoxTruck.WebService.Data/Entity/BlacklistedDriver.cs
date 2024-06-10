using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class BlacklistedDriver: BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
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
