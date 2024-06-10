using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission
{
    public class SubmissionFilterDto : FilterBase
    {
        public string SubmissionNumber { get; set; }
        public string InsuredName { get; set; }
        public string InsuredDBA { get; set; }
        public string Broker { get; set; }
        public DateTime? InceptionDateFrom { get; set; }
        public DateTime? InceptionDateTo { get; set; }
        public string Status { get; set; }
        public string PolicyStatus { get; set; }
        public string Underwriter { get; set; }
        public string Au_rep { get; set; }
        public string Au_psr { get; set; }
        public string Owner { get; set; }
        public string State { get; set; }
        public string SearchText { get; set; }
        public bool IsRenewal { get; set; }
        public bool IsDashboard { get; set; }
        public string Order { get; set; }
    }
}
