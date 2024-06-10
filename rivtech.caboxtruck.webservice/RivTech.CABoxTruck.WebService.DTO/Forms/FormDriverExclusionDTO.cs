using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DTO.Forms
{
    public class FormDriverExclusionDTO : IDialogUpdate
    {
        public List<FormDriverDTO> Drivers { get; set; } = new List<FormDriverDTO>();
        public DateTime? UpdatedDate { get; set; }

        public void SortDrivers()
        {
            if (Drivers?.Any() ?? false)
                return;
            Drivers = Drivers.OrderBy(x => x.FirstName).ToList();
        }
    }
}