using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Paddings;
using Programemein.Data.Entities;
using Programemein.Services.Scraping.Models;

namespace Programemein.Services.Memes
{
    public interface IMemeService
    {
        Task<int?> AddAsync(MemeModel meme, int sourceId);

        Meme GetOneNonUploadedToInstagramAsByteArray();

        bool IsExisting(string title);

        bool IsExisting(byte[] imageBytes);

        void MarkAsUploadedToInstagram(int id);
    }
}
