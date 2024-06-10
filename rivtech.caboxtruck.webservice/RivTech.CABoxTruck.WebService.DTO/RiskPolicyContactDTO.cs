using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskPolicyContactDTO
    {
        public Guid? Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public short TypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public bool IsActive { get; set; }
    }
}