using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;
using Quartz.Simpl;

namespace RivTech.CABoxTruck.WebService.Service.Services.BackgroundServices.JobFactories
{
    public class BoxtruckJobFactory : SimpleJobFactory
    {
        IServiceProvider _serviceProvider;

        public BoxtruckJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return (IJob)this._serviceProvider.GetService(bundle.JobDetail.JobType);
            }
            catch (Exception e)
            {
                throw new SchedulerException($"Problem while instantiating job '{bundle.JobDetail.Key}' from the Aspnet Core IOC.");
            }
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
