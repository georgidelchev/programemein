using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Programemein.Areas.Identity.IdentityHostingStartup))]
namespace Programemein.Areas.Identity
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