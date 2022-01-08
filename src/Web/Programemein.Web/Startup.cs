using System;
using System.Linq;
using System.Reflection;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Programemein.Common;
using Programemein.Data;
using Programemein.Data.Entities;
using Programemein.Services.Images;
using Programemein.Services.Instagram;
using Programemein.Services.Memes;
using Programemein.Services.RecurringJobs;
using Programemein.Web.Infrastructure.Extensions;

namespace Programemein.Web
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    this.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                    {
                        UseRecommendedIsolationLevel = true,
                        UsePageLocksOnDequeue = true,
                        DisableGlobalLocks = true,
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                    }).UseConsole());

            services.AddControllersWithViews();
            services.AddTransient<IImageProcessorService, ImageProcessorService>();
            services.AddTransient<IMemeService, MemeService>();
            services.AddTransient<IInstagramService, InstagramService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            app.PrepareDatabase();
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();

                this.LoadAllHangfireRecurringJobs(recurringJobManager, dbContext);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            _ = app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 2 });
            app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangfireAuthFilter() } });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }

        private void LoadAllHangfireRecurringJobs(IRecurringJobManager recurringJobManager, ApplicationDbContext dbContext)
        {
            foreach (var source in dbContext.Sources)
            {
                recurringJobManager.AddOrUpdate<GetLatestMemesCronJob>(
                    $"{source.OriginalUrl}",
                    x => x.StartWorking(source.TypeName, null),
                    "*/3 * * * *");
            }

            recurringJobManager.AddOrUpdate<UploadImageToInstagramCronJob>(
                $"Instagram Uploader",
                x => x.StartWorking(null),
                "*/59 * * * *");
        }

        private class HangfireAuthFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext dashboardContext)
                => dashboardContext.GetHttpContext().User.IsInRole("Admin");
        }
    }
}
