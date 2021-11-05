using System.Collections.Generic;
using Programemein.Services.Scraping.Models;

namespace Programemein.Services.Scraping.Sources
{
    public interface ISource
    {
        public string BaseUrl { get; set; }

        IEnumerable<MemeModel> GetAllMemes();

        IEnumerable<MemeModel> GetRecentMemes();

        IEnumerable<MemeModel> GetUrls(string url, string anchorTag, int count = 0);
    }
}
