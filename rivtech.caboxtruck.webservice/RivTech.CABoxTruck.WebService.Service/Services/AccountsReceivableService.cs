using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class AccountsReceivableService : IAccountsReceivableService
    {
        private readonly IAccountsReceivableRepository _accountsReceivableRepository;
        public AccountsReceivableService(IAccountsReceivableRepository accountsReceivableRepository)
        {
            _accountsReceivableRepository = accountsReceivableRepository;
        }

        public async  Task<List<AccountsReceivableDTO>> SearchAccountsReceivable(AccountsReceivableInputDTO data)
        {
            return await _accountsReceivableRepository.SearchAcountsRecievableAsync(data);
        }
    }
}
