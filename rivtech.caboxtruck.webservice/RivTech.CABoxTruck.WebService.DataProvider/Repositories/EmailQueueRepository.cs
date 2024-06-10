using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.ExternalData.Context;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class EmailQueueRepository : ExternalBaseRepository<EmailQueue, Guid>, IEmailQueueRepository
    {
        public EmailQueueRepository(AppDbExternalContext context)
           : base(context)
        {
        }
    }
}
