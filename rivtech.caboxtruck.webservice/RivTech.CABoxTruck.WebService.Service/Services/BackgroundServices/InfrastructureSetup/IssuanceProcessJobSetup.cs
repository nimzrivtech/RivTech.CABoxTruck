using Microsoft.Extensions.Options;
using Quartz;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;

namespace RivTech.CABoxTruck.WebService.Service.BackgroundServices.InfrastructureSetup
{
    public class IssuanceProcessJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create($"{nameof(IssuanceProcessJob)}-Job", $"{nameof(IssuanceProcessJob)}-Group");
            options.AddJob<IssuanceProcessJob>(opts => opts.WithIdentity(jobKey).StoreDurably(true));
        }
    }
}
