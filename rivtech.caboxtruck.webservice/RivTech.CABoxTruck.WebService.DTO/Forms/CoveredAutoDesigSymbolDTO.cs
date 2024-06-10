using System;

namespace RivTech.CABoxTruck.WebService.DTO.Forms
{
    public class CoveredAutoDesigSymbolDTO : IDialogUpdate
    {
        public string CoveredAutoText { get; set; }
        public string TextForEndorsement { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

