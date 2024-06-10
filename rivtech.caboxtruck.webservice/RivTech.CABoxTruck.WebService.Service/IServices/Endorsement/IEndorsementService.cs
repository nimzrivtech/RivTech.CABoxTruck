using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IEndorsementService
    {
        Task<EndorsementPremiumChangeSummaryDTO> GetPremiumChanges(Guid riskDetailId);
        Task CalculatePremiumChanges(Guid riskDetailId, DateTime effectiveDate, int optionNumber);
        Task UpdateEffectiveDate(Guid riskDetailId, DateTime effectiveDate);
        Task UpdateEffectiveDateByButton(Guid riskDetailId, DateTime effectiveDate);
        Task<string> GeneratePolicyNumber(Guid riskDetailId);
        Task<int> GetNextEndorsementNumber(Guid riskDetailId);
        Task<bool> ProcessEndorsement(Guid riskId);
        Task GetEndorsementChanges(Guid riskId);
        Task<bool> HasEffectiveDatePromptedAsync(Guid riskId);
        Task UpdateEffectiveDatePromptedAsync(Guid riskId, bool prompted);
        Task<bool> ProcessEndorsement(Guid riskId, NotificationFilterDTO notificationFilterDTO);
        Task<bool> ProcessEndorsementDefault(Guid riskId, NotificationFilterDTO notificationFilterDTO);
    }
}
