using System;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.Applicant
{
    public interface IFilingsInformationService
    {
        Task<FilingsInformationDTO> GetAsync(Guid id);

        Task<string> UpdateAsync(FilingsInformationDTO model);
    }
}