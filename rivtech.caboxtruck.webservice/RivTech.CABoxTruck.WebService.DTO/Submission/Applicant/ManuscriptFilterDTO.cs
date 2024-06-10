using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.Applicant
{
    public class ManuscriptFilterDTO
    {
        public Guid? RiskDetailId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string Order { get; set; }
        public string Keyword { get; set; }
        public bool isPolicy { get; set; }
    }
}
