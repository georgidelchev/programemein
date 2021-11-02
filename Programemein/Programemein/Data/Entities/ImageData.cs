using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programemein.Data.Entities
{
    public class ImageData
    {
        public Guid Id { get; set; }

        public string OriginalFileName { get; set; }

        public string OriginalType { get; set; }

        public byte[] OriginalContent { get; set; }

        public byte[] ThumbnailContent { get; set; }

        public byte[] InstagramContent { get; set; }
    }
}
