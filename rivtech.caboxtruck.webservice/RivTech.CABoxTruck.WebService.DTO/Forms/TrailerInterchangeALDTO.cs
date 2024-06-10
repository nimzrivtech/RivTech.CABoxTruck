using System;

namespace RivTech.CABoxTruck.WebService.DTO.Forms
{
    public class TrailerInterchangeALDTO : IDialogUpdate
    {
        public short? TrailerInterchangeLimitId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

