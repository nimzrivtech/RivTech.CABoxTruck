using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Questions
{
    public class QuestionDTO
    {
        public Int16 Id { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public Int16 CategoryId { get; set; }
        public Int16 SectionId { get; set; }
    }
}
