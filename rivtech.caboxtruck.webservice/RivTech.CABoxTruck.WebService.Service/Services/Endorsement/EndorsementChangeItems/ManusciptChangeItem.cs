using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class ManusciptChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(ManusciptChangeItem);

        public string DisplayText => "";
        public List<RiskManuscriptsDTO> RiskManuscripts { get; set; }
    }
}
