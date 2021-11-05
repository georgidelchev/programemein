using Microsoft.AspNetCore.Hosting;
using Programemein.Web.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace Programemein.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}