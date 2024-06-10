using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class DriverRequirementDTO : IDialogUpdate
    {
        public string Requirement { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}