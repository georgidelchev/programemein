using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Programemein.Common;
using Programemein.Data;
using Programemein.Services.Memes;
using Programemein.Services.Scraping.Sources;

namespace Programemein.Services.RecurringJobs
{
    public class GetLatestMemesCronJob
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMemeService memeService;

        public GetLatestMemesCronJob(
            ApplicationDbContext dbContext,
            IMemeService memeService)
        {
            this.dbContext = dbContext;
            this.memeService = memeService;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task StartWorking(string typeName, PerformContext context)
        {
            var source = this.dbContext
                .Sources
                .FirstOrDefault(x => x.TypeName == typeName);

            if (source == null)
            {
                throw new Exception($"Source {typeName} has not found in the database!");
            }

            var instance = ReflectionHelpers.GetInstance<Source>(typeName);

            var memes = instance
                .GetRecentMemes()
                .ToList();

            foreach (var meme in memes.WithProgress(context.WriteProgressBar()))
            {
                var newsId = await this.memeService.AddAsync(meme, source.Id);

                if (newsId.HasValue && meme != null)
                {
                    context.WriteLine($"[ID:{newsId}] Successfully imported news with title: {meme.Title}");
                }
            }
        }
    }
}
