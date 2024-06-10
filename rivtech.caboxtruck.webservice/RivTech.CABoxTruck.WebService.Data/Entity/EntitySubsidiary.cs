using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class EntitySubsidiary : BaseEntity<Guid>
    {
        public EntitySubsidiary()
        {
            EffectiveDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(5);
            AddProcessDate = DateTime.Now;
        }
        public Guid EntityId { get; set; }
        public bool IsSubsidiaryCompany { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public short? TypeOfBusinessId { get; set; }
        public bool IsIncludedInInsurance { get; set; }
        public int? NumVechSizeComp { get; set; }
        public bool IsCurrentlyOwned { get; set; }
        public int? NameYearOperations { get; set; }
        public bool IsOthersAllowedOperate { get; set; }
        public bool IsOthersAllowedPrevOperate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AddProcessDate { get; set; }
        public long AddedBy { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
        public long? RemovedBy { get; set; }

        public Entity Entity { get; set; }
    }
}
