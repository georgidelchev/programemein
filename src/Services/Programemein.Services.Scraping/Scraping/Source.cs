using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Programemein.Services.Scraping.Scraping
{
    public abstract class Source : ISource
    {
        public virtual string BaseUrl { get; set; }

        private HtmlParser Parser { get; set; } = new();

        public virtual IEnumerable<MemeModel> GetAllMemes()
            => new List<MemeModel>();

        public abstract IEnumerable<MemeModel> GetRecentMemes();

        protected abstract MemeModel ParseDocument(IDocument document, string url);

        public IEnumerable<MemeModel> GetUrls(string url, string anchorTag, int count = 0)
        {
            var webClient = new WebClient();

            var html = webClient.DownloadString($"{this.BaseUrl}{url}");
            var document = this.Parser.ParseDocument(html);

            var links = document
                .QuerySelectorAll(anchorTag)
                .Where(l => !string.IsNullOrWhiteSpace(l.Attributes["href"]?.Value))
                .Select(l => this.RebuildGivenUrl(l?.Attributes["href"]?.Value))
                .Distinct()
                .ToList();

            if (count > 0)
            {
                links = links.Take(count).ToList();
            }

            return links.Select(this.GetMemes)
                .Where(x => x != null)
                .ToList();
        }

        protected MemeModel GetMemes(string url)
        {
            var webClient = new WebClient();

            var html = webClient.DownloadString(url);

            IHtmlDocument document;

            try
            {
                document = this.Parser.ParseDocument(html);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    switch ((ex.Response as HttpWebResponse)?.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.InternalServerError:
                            return null;
                    }
                }

                throw;
            }

            var meme = this.ParseDocument(document, url);
            if (meme == null)
            {
                return null;
            }

            meme.Title = meme.Title?.Trim().Replace("  ", " ");

            if (meme.CreatedOn > DateTime.Now)
            {
                meme.CreatedOn = DateTime.Now;
            }

            if (meme.CreatedOn.Date == DateTime.UtcNow.Date &&
                meme.CreatedOn.Hour == 0 &&
                meme.CreatedOn.Minute == 0)
            {
                meme.CreatedOn = DateTime.Now;
            }

            meme.OriginalUrl = url.Trim();

            if (meme.ImageUrl != null)
            {
                meme.ImageUrl = meme.ImageUrl.Trim();
            }

            return meme;
        }

        private string RebuildGivenUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            return !Uri.TryCreate(new Uri(this.BaseUrl), url, out var result) ? url : result.ToString();
        }
    }
}
