using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class EntityAddressDTO
    {
        public Guid? Id { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? AddressId { get; set; }
        public Int16? AddressTypeId { get; set; }
        public bool? IsMainGaragingAddress { get; set; }
        public AddressDTO Address { get; set; }
        public Guid RiskDetailId { get; set; }

        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
