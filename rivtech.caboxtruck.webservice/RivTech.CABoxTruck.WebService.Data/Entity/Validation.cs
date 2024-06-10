using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Validation : BaseEntity<Int16>
    {
        public Int16 ValidationTypeId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public LvValidationType ValidationType { get; set; }
    }
}
