using Autofac;
using RivTech.CABoxTruck.WebService.DomainEntities;
using RivTech.CABoxTruck.WebService.DomainEntities.Security;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services;
using RivTech.CABoxTruck.WebService.Service.Services.DataFixes.Cancellations;
using RivTech.CABoxTruck.WebService.Service.Services.Fees;
using RivTech.CABoxTruck.WebService.Service.Services.Security;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;

namespace RivTech.CABoxTruck.WebService.StartUp.AutofacModules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SubmissionService>().As<ISubmissionService>().InstancePerLifetimeScope();
            builder.RegisterType<PremiumEarnedTaxReportService>().As<IPremiumEarnedTaxReportService>().InstancePerLifetimeScope();
            builder.RegisterType<BorderauxReportService>().As<IBorderauxReportService>().InstancePerLifetimeScope();

            builder.RegisterType<BackgroundServiceChecker>().As<IBackgroundServiceChecker>().InstancePerLifetimeScope();
            builder.RegisterType<TokenProvider>().As<ITokenProvider>().InstancePerLifetimeScope();

            builder.RegisterType<FeeService>().As<IFeeService>().InstancePerLifetimeScope();

            builder.RegisterType<CancellationPDBreakdownDataFixer>().As<ICancellationPDBreakdownDataFixer>().InstancePerLifetimeScope();

            builder.RegisterType<AgingReportService>().As<IAgingReportService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
