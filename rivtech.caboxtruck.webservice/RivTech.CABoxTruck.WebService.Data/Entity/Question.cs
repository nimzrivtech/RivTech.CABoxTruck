using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Question : BaseEntity<Int16>
    {
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public Int16 CategoryId { get; set; }
        public Int16 SectionId { get; set; }

        public LvQuestionCategory Category { get; set; }
        public LvQuestionSection Section { get; set; }
    }
}