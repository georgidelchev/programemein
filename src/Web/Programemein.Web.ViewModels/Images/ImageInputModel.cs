using System.IO;

namespace Programemein.Web.ViewModels.Images
{
    public class ImageInputModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public Stream Content { get; set; }
    }
}
