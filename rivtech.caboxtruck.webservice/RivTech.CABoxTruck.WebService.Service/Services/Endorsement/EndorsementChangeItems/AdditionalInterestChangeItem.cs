using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class AdditionalInterestChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(AdditionalInterestChangeItem);

        public string DisplayText => "";
        public List<RiskAdditionalInterestDTO> AdditionalInterests { get; set; }
    }
}
