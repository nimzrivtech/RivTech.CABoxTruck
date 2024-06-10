using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class RatingFactorsChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(RatingFactorsChangeItem);
        public string DisplayText => "";
        public decimal? ALScheduleRF { get; set; }
        public decimal? PDScheduleRF { get; set; }
        public decimal? CargoScheduleRF { get; set; }
        public decimal? GLScheduleRF { get; set; }

        public bool HasChanges()
        {
            return ALScheduleRF != null
                || PDScheduleRF != null
                || CargoScheduleRF != null
                || GLScheduleRF != null;
        }
    }
}
