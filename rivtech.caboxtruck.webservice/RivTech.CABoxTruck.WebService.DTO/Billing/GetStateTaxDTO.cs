using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class GetStateTaxDTO
    {
        public DateTime EffectiveDate { get; set; }
        public string StateCode { get; set; }
    }
}
