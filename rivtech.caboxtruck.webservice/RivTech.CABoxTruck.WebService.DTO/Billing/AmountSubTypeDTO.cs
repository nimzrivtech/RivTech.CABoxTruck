using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class AmountSubTypeDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        
        public AmountType AmountType { get; set; }
    }

    public class AmountType
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
