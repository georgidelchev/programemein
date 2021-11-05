using System;
using System.Collections.Generic;

namespace Programemein.Services.Scraping
{
    public class MemeModel
    {
        public MemeModel(
            string title,
            DateTime createdOn,
            string imageUrl,
            string originalUrl,
            IEnumerable<string> tags)
        {
            this.Title = title;
            this.CreatedOn = createdOn;
            this.ImageUrl = imageUrl;
            this.OriginalUrl = originalUrl;
            this.Tags = tags;
        }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public IEnumerable<string> Tags { get; }
    }
}
