using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class EndorsementFormDTO
    {
        public string TemplateName { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }

        public List<RiskFormDTO> RiskForms { get; set; }
        public RiskAdditionalInterestDTO AIWithNotice { get; set; }
    }
}

