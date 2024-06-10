using Microsoft.Extensions.Options;
using Quartz;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;

namespace RivTech.CABoxTruck.WebService.Service.BackgroundServices.InfrastructureSetup
{
    public class EndorsementProcessJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create($"{nameof(EndorsementProcessJob)}-Job", $"{nameof(EndorsementProcessJob)}-Group");
            options.AddJob<EndorsementProcessJob>(opts => opts.WithIdentity(jobKey).StoreDurably(true));

            //options.AddTrigger(opts => opts
            //    .ForJob(jobKey)
            //    .WithIdentity($"{nameof(EndorsementProcessJob)}-trigger", $"{nameof(EndorsementProcessJob)}-Group")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(2)
            //        .RepeatForever())
            //);
        }
    }
}
