using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class EntityAddress : BaseEntity<Guid>, ICreatedData
    {
        public EntityAddress()
        {
            EffectiveDate = DateTime.Now.Date;
            ExpirationDate = DateTime.Now.Date.AddYears(5);
        }
        public Guid EntityId { get; set; }
        public Guid AddressId { get; set; }
        public short? AddressTypeId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
        public long? RemovedBy { get; set; }
        public bool? IsMainGaragingAddress { get; set; }


        public Entity Entity { get; set; }
        public Address Address { get; set; }
    }
}
