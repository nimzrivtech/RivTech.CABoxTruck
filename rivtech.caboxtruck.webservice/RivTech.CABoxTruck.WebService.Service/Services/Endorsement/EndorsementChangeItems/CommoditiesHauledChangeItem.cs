using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class CommoditiesHauledChangeItem : IEndorsementChangeItem
    {
        public const string TYPE = nameof(CommoditiesHauledChangeItem);

        public string Type => TYPE;

        public string DisplayText => $"";
        public List<RiskSpecificCommoditiesHauledDto> CommoditiesHauled { get; set; }
    }
}
