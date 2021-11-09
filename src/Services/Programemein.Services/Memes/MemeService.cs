using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Programemein.Data;
using Programemein.Data.Entities;
using Programemein.Services.Images;
using Programemein.Services.Scraping.Models;
using Programemein.Web.ViewModels.Images;

namespace Programemein.Services.Memes
{
    public class MemeService : IMemeService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IImageProcessorService imageProcessorService;

        public MemeService(
            ApplicationDbContext dbContext,
            IImageProcessorService imageProcessorService)
        {
            this.dbContext = dbContext;
            this.imageProcessorService = imageProcessorService;
        }

        public async Task<int?> AddAsync(MemeModel meme, int sourceId)
        {
            var imageBytes = new WebClient().DownloadData(meme.ImageUrl);

            if (IsExisting(meme.Title) || IsExisting(imageBytes))
            {
                return null;
            }

            var memeToAdd = new Meme
            {
                CreatedOn = DateTime.UtcNow,
                OriginalUrl = meme.OriginalUrl,
                SourceId = sourceId,
                Title = meme.Title,
            };

            await this.dbContext.Memes.AddAsync(memeToAdd);
            await this.dbContext.SaveChangesAsync();

            var imageInputModel = new ImageInputModel
            {
                Content = new MemoryStream(imageBytes),
                Name = meme.Title,
                MemeId = memeToAdd.Id,
                Type = "PNG",
            };

            await this.imageProcessorService.Process(imageInputModel);

            foreach (var tag in meme.Tags)
            {
                memeToAdd.Tags.Add(new Tag { Name = tag });
            }

            this.dbContext.Memes.Update(memeToAdd);
            await this.dbContext.SaveChangesAsync();

            return memeToAdd.Id;
        }

        public bool IsExisting(string title)
            => this.dbContext.Memes.Any(m => m.Title == title);

        public bool IsExisting(byte[] imageBytes)
            => this.dbContext.Memes.Any(m => m.ImageData.OriginalContent == imageBytes);
    }
}
