using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class FieldVersion : BaseEntity<string>
    {
        public string FieldName { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
