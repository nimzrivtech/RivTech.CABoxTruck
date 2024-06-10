using System;

namespace RivTech.CABoxTruck.WebService.DTO.Forms
{
    public class PollutionCoverageDTO : IDialogUpdate
    {
        public short? PollutionCovLimitId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

