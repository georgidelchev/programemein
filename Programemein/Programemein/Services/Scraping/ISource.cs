using System.Collections.Generic;

namespace Programemein.Services.Scraping
{
    public interface ISource
    {
        public string BaseUrl { get; set; }

        IEnumerable<MemeModel> GetAllMemes();

        IEnumerable<MemeModel> GetRecentMemes();

        IEnumerable<MemeModel> GetUrls(string url, string anchorTag, int count = 0);
    }
}
