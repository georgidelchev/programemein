using Programemein.Models.Images;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Programemein.Services.Images
{
    public interface IImageProcessorService
    {
        Task Process(IEnumerable<ImageInputModel> images);

        Task Process(ImageInputModel image);

        Task<Stream> GetOriginal(string id);

        Task<Stream> GetThumbnail(string id);

        Task<Stream> GetInstagram(string id);
    }
}
