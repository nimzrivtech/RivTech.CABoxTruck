using Autofac;
using RivTech.CABoxTruck.WebService.Service.Services.Forms;

namespace RivTech.CABoxTruck.WebService.StartUp.DependencyInjection
{
    public class DriverServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DriverExclusionEndorsementFormService>().As<IDriverExclusionEndorsementFormService>().InstancePerLifetimeScope();
        }
    }
}
