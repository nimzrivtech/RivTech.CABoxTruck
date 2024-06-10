using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvReasonMove : Enumeration<short>
    {
        public LvReasonMove()
        {
            this.IsActive = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Int16 Id { get; set; }
    }
}