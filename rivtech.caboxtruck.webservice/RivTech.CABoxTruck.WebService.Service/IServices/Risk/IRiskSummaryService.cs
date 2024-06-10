using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Risk
{
    public interface IRiskSummaryService
    {
        Task<SummaryListPagingDTO> GetFilteredSummaryListAsync(SubmissionFilterDto filter);
    }
}
