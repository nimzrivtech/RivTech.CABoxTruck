using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class SaveEntitySubsidiaryDTO
    {
        public Guid? Id { get; set; }
        public Guid? RiskDetailId { get; set; }
        public Guid? EntityId { get; set; }
        public bool IsSubsidiaryCompany { get; set; }
        public string Name { get; set; }
        public short TypeOfBusinessId { get; set; }
        public int? NumVechSizeComp { get; set; }
        public string Relationship { get; set; }
        public bool IsIncludedInInsurance { get; set; }  
        public bool IsCurrentlyOwned { get; set; }
        public int? NameYearOperations { get; set; }
        public bool IsOthersAllowedOperate { get; set; }
        public bool IsOthersAllowedPrevOperate { get; set; }
    }
}
