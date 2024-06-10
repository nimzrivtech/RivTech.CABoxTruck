using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.Applicant
{
    public class ManuscriptPagingDTO
    {   
        public PaginationDetails PaginationDetails { get; set; }
        public List<RiskManuscriptsDTO> RiskManuscript { get; set; }
    }
}
