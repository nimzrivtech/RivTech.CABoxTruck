using Microsoft.Extensions.Options;
using Quartz;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;

namespace RivTech.CABoxTruck.WebService.Service.BackgroundServices.InfrastructureSetup
{
    public class ResetProcessJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create($"{nameof(ResetProcessJob)}-Job", $"{nameof(ResetProcessJob)}-Group");
            options.AddJob<ResetProcessJob>(opts => opts.WithIdentity(jobKey).StoreDurably(true));

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
