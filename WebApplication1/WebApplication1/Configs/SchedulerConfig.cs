using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace WebApplication1.Configs
{
    public class SchedulerConfig
    {
        private IBackgroundJobClient _backgroundJobClient;
        private IRecurringJobManager _recurringJobManager;

        public SchedulerConfig(IBackgroundJobClient backgroundJob, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJob;
            _recurringJobManager = recurringJobManager;
        }

        public void SchedulerActivation()
        {
            _backgroundJobClient.Schedule(() => Console.WriteLine($"Hangfire test. Run at: {DateTime.Now}"), TimeSpan.FromMinutes(1));
            _recurringJobManager.RemoveIfExists("post-fakejson");
            _recurringJobManager.AddOrUpdate<RequestService>("post-fakejson", x => x.PostToFakeJson(), Cron.Minutely);
        }
    }
}
