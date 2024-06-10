using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class TerritoryExclusionDTO : IDialogUpdate
    {
        public string States { get; set; }

        public bool StatesDefault { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}