using System.Threading.Tasks;

namespace Programemein.Services.Instagram
{
    public interface IInstagramService
    {
        Task<string> UploadAsync();
    }
}
