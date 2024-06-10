using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class FormLossPayeeDTO : IDialogUpdate
    {
        public string DescriptionOfAutos { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}