using System;
using System.ComponentModel.DataAnnotations;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class CoveredSymbol : BaseEntity<Int16>
    {
        public CoveredSymbol()
        {
            this.EffectiveDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now.AddYears(5);
            this.AddProcessDate = DateTime.Now;
        }

        public string Name { get; set; }
        public Int32 ProgramId { get; set; }
        public Int16 StateId { get; set; }

        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AddProcessDate { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
    }
}

