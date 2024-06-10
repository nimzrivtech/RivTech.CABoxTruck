using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class EntityContact : BaseEntity<Guid>
    {
        public EntityContact()
        {
            EffectiveDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(5);
            AddProcessDate = DateTime.Now;
        }
        public Guid EntityId { get; set; }
        public short? ContactTypeId { get; set; }
        public short? CompanyPositionId { get; set; }
        public short? YearsInPosition { get; set; }
        public string PhoneExtension { get; set; }
        public string EmailAddress { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AddProcessDate { get; set; }
        public long AddedBy { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
        public long? RemovedBy { get; set; }

        public Entity Entity { get; set; }
    }
}
