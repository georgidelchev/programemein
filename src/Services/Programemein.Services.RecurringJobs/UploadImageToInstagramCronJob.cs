using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Programemein.Services.Instagram;

namespace Programemein.Services.RecurringJobs
{
    public class UploadImageToInstagramCronJob
    {
        private readonly IInstagramService instagramService;

        public UploadImageToInstagramCronJob(IInstagramService instagramService)
        {
            this.instagramService = instagramService;
        }

        [AutomaticRetry(Attempts = 2)]
        public async Task StartWorking(PerformContext context)
            => context.WriteLine(await this.instagramService.UploadAsync());
    }
}
