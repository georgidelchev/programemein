using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mime;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Programemein.Web.ViewModels.Images;
using Microsoft.Extensions.DependencyInjection;
using Programemein.Data;
using Programemein.Data.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Programemein.Services.Images
{
    public class ImageProcessorService : IImageProcessorService
    {
        private const int ThumbnailWidth = 300;
        private const int InstagramWidth = 1280;

        private readonly IServiceScopeFactory _serviceFactory;
        private readonly ApplicationDbContext _dbContext;

        public ImageProcessorService(
            IServiceScopeFactory serviceFactory,
            ApplicationDbContext dbContext)
        {
            _serviceFactory = serviceFactory;
            _dbContext = dbContext;
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
                    // ignored
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
                // ignored
            }
        }

        public Task<Stream> GetOriginal(string id)
        {
            return GetImageData(id, "Original");
        }

        public Task<Stream> GetThumbnail(string id)
        {
            return GetImageData(id, "Thumbnail");
        }

        public Task<Stream> GetInstagram(string id)
        {
            return GetImageData(id, "Instagram");
        }

        private async Task<Stream> GetImageData(string id, string size)
        {
            var database = _dbContext.Database;

            var dbConnection = (SqlConnection)database.GetDbConnection();

            var command = new SqlCommand(
                $"SELECT {size}Content FROM Images WHERE Id = @id",
                dbConnection
            );

            command.Parameters.Add(new SqlParameter("@id", id));

            await dbConnection.OpenAsync();

            var reader = await command.ExecuteReaderAsync();

            Stream result = null;

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    result = reader.GetStream(0);
                }
            }

            await reader.CloseAsync();
            await dbConnection.CloseAsync();

            return result;
        }

        private async Task Processor(ImageInputModel image)
        {
            using var imageResult = await Image.LoadAsync(image.Content);

            var original = await ImageToByteArray(imageResult, imageResult.Width);
            var thumbnail = await ImageToByteArray(imageResult, ThumbnailWidth);
            var instagram = await ImageToByteArray(imageResult, InstagramWidth);

            var dbContext = _serviceFactory
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

            image.Mutate(i => i.Resize(new Size(width, height)));

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
