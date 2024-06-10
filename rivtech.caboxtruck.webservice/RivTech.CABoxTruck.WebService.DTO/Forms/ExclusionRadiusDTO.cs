using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class ExclusionRadiusDTO : IDialogUpdate
    {
        public int Radius { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}