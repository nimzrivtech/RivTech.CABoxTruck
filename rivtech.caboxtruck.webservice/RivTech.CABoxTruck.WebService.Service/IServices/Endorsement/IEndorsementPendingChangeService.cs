using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IEndorsementPendingChangeService
    {
        Task<List<EndorsementPendingChangeDTO>> GetAll(Guid riskDetailId);
    }
}
