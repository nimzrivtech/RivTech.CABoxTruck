using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class EntitySubsidiaryDTO 
    {
        public Guid? Id { get; set; }
        public Guid? EntityId { get; set; }
        public string Name { get; set; }
        public short TypeOfBusinessId { get; set; }
        public int? NumVechSizeComp { get; set; }
        public string Relationship { get; set; }
        public bool IsIncludedInInsurance { get; set; }  
        public bool IsCurrentlyOwned { get; set; }
        public int? NameYearOperations { get; set; }
        public bool IsOthersAllowedOperate { get; set; }
        public bool IsOthersAllowedPrevOperate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
