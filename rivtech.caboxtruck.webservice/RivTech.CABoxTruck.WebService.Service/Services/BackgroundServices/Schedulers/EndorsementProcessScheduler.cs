using DocumentFormat.OpenXml.Office2013.Excel;
using NPOI.SS.Formula.Functions;
using Quartz;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;
using RivTech.CABoxTruck.WebService.Service.Services.BackgroundServices.ISchedulers;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.BackgroundServices.Schedulers
{
    public class EndorsementProcessScheduler : IEndorsementProcessScheduler
    {
        public static readonly JobKey jobKey = new JobKey($"{nameof(EndorsementProcessJob)}-Job", $"{nameof(EndorsementProcessJob)}-Group");
        private readonly IScheduler _scheduler;

        public EndorsementProcessScheduler(IScheduler scheduler)
        { 
            _scheduler = scheduler;
        }

        public async Task Add()
        {
            IJobDetail job = JobBuilder.Create<EndorsementProcessJob>()
                                .WithIdentity(jobKey)
                                .Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //                    .WithIdentity($"{nameof(EndorsementProcessJob)}-Trigger", $"{nameof(EndorsementProcessJob)}-Group")
            //                    .StartNow()
            //                    .Build();

            //await _scheduler.ScheduleJob(job, trigger);

            var replace = true;
            var durable = true;

            await _scheduler.AddJob(job, replace, durable);
        }
    }
}
