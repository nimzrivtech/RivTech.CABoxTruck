using Autofac;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Endorsements;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;

namespace RivTech.CABoxTruck.WebService.StartUp.AutofacModules
{
    public class EndorsementServicesModule  : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndorsementProcessStatusLogService>().As<IEndorsementProcessStatusLogService>();
            builder.RegisterType<EndorsementProcessStatusLogRepository>().As<IEndorsementProcessStatusLogRepository>();

            builder.RegisterType<EndorsementProcessQueueRepository>().As<IEndorsementProcessQueueRepository>();
            builder.RegisterType<EndorsementProcessQueueService>().As<IEndorsementProcessQueueService>();

            base.Load(builder);
        }
    }
}
