using System.Threading.Tasks;
using Programemein.Services.Scraping.Models;

namespace Programemein.Services.Memes
{
    public interface IMemeService
    {
        Task<int?> AddAsync(MemeModel meme, int memeId);
    }
}
