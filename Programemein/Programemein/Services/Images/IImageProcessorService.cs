using Programemein.Models.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programemein.Services.Images
{
    public interface IImageProcessorService
    {
        Task Process(IEnumerable<ImageInputModel> images);

        Task Process(ImageInputModel image);
    }
}
