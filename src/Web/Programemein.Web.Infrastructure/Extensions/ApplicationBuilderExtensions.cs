using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Programemein.Data;
using Programemein.Data.Entities;

namespace Programemein.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);
            SeedAdministrator(services);
            SeedSources(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<ApplicationDbContext>();

            data.Database.Migrate();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Admin"))
                    {
                        return;
                    }

                    var role = new IdentityRole {Name = "Admin"};

                    await roleManager.CreateAsync(role);

                    const string adminUsername = "adminaaa";
                    const string adminEmail = "adminEmailProgramein@gmail.com";
                    const string adminPassword = "@Programeinadminaccount343";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminUsername,
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedSources(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

            var sources = new List<(string TypeName, string Url)>
            {
                ("Programemein.Services.Scraping.Sources.ProgrammerIo", "www.programmer.io"),
            };

            foreach (var source in sources)
            {
                if (dbContext.Sources.Any(s => s.TypeName == source.TypeName))
                {
                    continue;
                }

                var sourceToAdd = new Source()
                {
                    TypeName = source.TypeName,
                    OriginalUrl = source.Url,
                };

                dbContext.Sources.Add(sourceToAdd);
                dbContext.SaveChanges();
            }
        }
    }
}
