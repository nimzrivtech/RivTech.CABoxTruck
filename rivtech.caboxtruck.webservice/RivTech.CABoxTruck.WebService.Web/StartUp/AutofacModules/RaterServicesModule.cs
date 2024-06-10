using Autofac;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;

namespace RivTech.CABoxTruck.WebService.StartUp.AutofacModules
{
    public class RaterServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RaterService>().As<IRaterService>().InstancePerLifetimeScope();
            builder.RegisterType<RaterInputValuesFactory>().As<IRaterInputValuesFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ExperienceRaterVersionService>().As<IExperienceRaterVersionService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
