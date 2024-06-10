using Autofac;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;

namespace RivTech.CABoxTruck.WebService.StartUp.AutofacModules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PremiumEarnedTaxReportRepository>().As<IPremiumEarnedTaxReportRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BorderauxReportRepository>().As<IBorderauxReportRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AgingReportRepository>().As<IAgingReportRepository>().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
