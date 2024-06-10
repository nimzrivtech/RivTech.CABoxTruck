using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class FieldVersionDTO
    {
        public Guid Id { get; set; }
        public string FieldName { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
