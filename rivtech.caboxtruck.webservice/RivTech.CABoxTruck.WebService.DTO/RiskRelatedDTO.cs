using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskRelatedDTO
    {
        public Guid Id { get; set; }
        public Guid? Current { get; set; }
        public string PolicyNumber { get; set; }
        public bool IsPolicy { get; set; }
    }
}
