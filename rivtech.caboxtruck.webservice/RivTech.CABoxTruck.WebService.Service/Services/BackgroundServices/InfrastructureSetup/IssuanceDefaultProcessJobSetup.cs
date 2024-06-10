using Microsoft.Extensions.Options;
using Quartz;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;

namespace RivTech.CABoxTruck.WebService.Service.BackgroundServices.InfrastructureSetup
{
    public class IssuanceDefaultProcessJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create($"{nameof(IssuanceDefaultProcessJob)}-Job", $"{nameof(IssuanceDefaultProcessJob)}-Group");
            options.AddJob<IssuanceDefaultProcessJob>(opts => opts.WithIdentity(jobKey).StoreDurably(true));
        }
    }
}
