using Microsoft.Extensions.DependencyInjection;
using Programemein.Models.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Programemein.Data;
using Programemein.Data.Entities;

namespace Programemein.Services.Images
{
    public class ImageProcessorService : IImageProcessorService
    {
        private const int ThumbnailWidth = 300;
        private const int InstagramWidth = 1280;

        private readonly IServiceScopeFactory serviceFactory;

        public ImageProcessorService(IServiceScopeFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public async Task Process(IEnumerable<ImageInputModel> images)
        {
            var tasks = images
            .Select(image => Task.Run(async () =>
            {
                try
                {
                    await Processor(image);
                }
                catch
                {

                }
            }));

            await Task.WhenAll(tasks);
        }

        public async Task Process(ImageInputModel image)
        {
            try
            {
                await Processor(image);
            }
            catch
            {

            }
        }

        private async Task Processor(ImageInputModel image)
        {
            using var imageResult = await Image.LoadAsync(image.Content);

            var original = await ImageToByteArray(imageResult, imageResult.Width);
            var thumbnail = await ImageToByteArray(imageResult, ThumbnailWidth);
            var instagram = await ImageToByteArray(imageResult, InstagramWidth);

            var dbContext = this.serviceFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

            var imageModel = new ImageData();
            imageModel.OriginalFileName = image.Name;
            imageModel.OriginalType = image.Type;
            imageModel.OriginalContent = original;
            imageModel.ThumbnailContent = thumbnail;

            await dbContext.Images.AddAsync(imageModel);
            await dbContext.SaveChangesAsync();
        }

        private async Task<byte[]> ImageToByteArray(Image image, int resizeWidth)
        {
            var width = image.Width;
            var height = image.Height;

            if (width > resizeWidth)
            {
                height = (int)((double)resizeWidth / width * height);
                width = resizeWidth;
            }

            image
                .Mutate(i => i
                    .Resize(new Size(width, height)));

            image.Metadata.ExifProfile = null;

            var stream = new MemoryStream();

            await image.SaveAsJpegAsync(stream, new JpegEncoder
            {
                Quality = 75
            });

            return stream.ToArray();
        }
    }
}
