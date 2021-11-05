using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AngleSharp.Dom;
using Programemein.Services.Scraping.Scraping;

namespace Programemein.Services.Scraping
{
    /// <summary>
    /// Programmer.io Source
    /// </summary>
    public class ProgrammerIo : Source
    {
        private const string PagingFragment = "page/{0}/";
        private const string AnchorTag = "h2.g1-beta a";
        private const int StartPage = 1;
        private const int EndPage = 850;
        private const int MemesCount = 10;

        public override string BaseUrl { get; set; } = "https://programmerhumor.io/";

        public override IEnumerable<MemeModel> GetRecentMemes()
            => this.GetUrls(string.Format(PagingFragment, StartPage), AnchorTag, MemesCount);

        public override IEnumerable<MemeModel> GetAllMemes()
        {
            for (var i = StartPage; i <= EndPage; i++)
            {
                var memes = GetUrls(string.Format(PagingFragment, i), AnchorTag);

                foreach (var meme in memes)
                {
                    yield return meme;
                }
            }
        }

        protected override MemeModel ParseDocument(IDocument document, string url)
        {
            var title = document
                .QuerySelector("h1.g1-mega")
                ?.InnerHtml;

            // 2021-05-02 T 05:07:17 +00:00
            var createdOnAsString = document
                .QuerySelectorAll("time.entry-date")
                .Select(m => m.GetAttribute("datetime"))
                .FirstOrDefault()
                ?.Split('T')[0];

            var createdOn = DateTime.ParseExact(createdOnAsString ?? string.Empty, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None);

            var imageUrl = document
                .QuerySelector("picture img")
                ?.GetAttribute("src");

            var tags = document
                .QuerySelectorAll("span.entry-tags-inner a")
                .Select(t => "#" + t.InnerHtml);

            return new MemeModel(title, createdOn, imageUrl, url, tags);
        }
    }
}
