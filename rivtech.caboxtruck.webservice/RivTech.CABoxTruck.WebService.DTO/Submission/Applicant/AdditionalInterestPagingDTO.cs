using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.Applicant
{
    public class AdditionalInterestPagingDTO
    {
        public PaginationDetails PaginationDetails { get; set; }
        public List<RiskAdditionalInterestDTO> AdditionalInterest { get; set; }
    }
}
