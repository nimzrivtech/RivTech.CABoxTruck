using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class QuestionValidation
    {
        public Int16 QuestionId { get; set; }
        public Question Question { get; set; }

        public Int16 ValidationId { get; set; }
        public Validation Validation { get; set; }
    }
}
