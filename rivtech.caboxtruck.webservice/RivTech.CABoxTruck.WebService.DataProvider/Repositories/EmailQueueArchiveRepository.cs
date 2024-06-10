using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.ExternalData.Context;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class EmailQueueArchiveRepository : ExternalBaseRepository<EmailQueueArchive, Guid>, IEmailQueueArchiveRepository
    {
        public EmailQueueArchiveRepository(AppDbExternalContext context)
           : base(context)
        {
        }
    }
}
