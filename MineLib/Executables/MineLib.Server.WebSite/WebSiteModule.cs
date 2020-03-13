using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MineLib.Server.WebSite.BackgroundServices;
using MineLib.Server.WebSite.Data;
using MineLib.Server.WebSite.Models;
using MineLib.Server.WebSite.Repositories;
using MineLib.Server.WebSite.Services;

using System;

using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MineLib.Server.WebSite
{
    public class UsersEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //context.Services.AddAbpDbContext<UserContext>(options =>
            //{
            //    /* Remove "includeAllEntities: true" to create
            //     * default repositories only for aggregate roots */
            //    options.AddDefaultRepositories(includeAllEntities: true);
            //});

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also BookStoreMigrationsDbContextFactory for EF Core tooling. */
                options.UsePostgreSql();
            });


            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.AddDbContext<UserContext>(options => options
                .ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
                .UseNpgsql(configuration.GetConnectionString("Users")));

            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UserContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
        }
    }

    //[DependsOn(typeof(WebSiteDatabaseModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(QServerModule))]
    public class WebSiteModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<ServiceOptions>(o => o.Name = "WebSite");

            services.AddNpgSqlMetrics("ClassicServers", configuration.GetConnectionString("ClassicServers"));
            services.AddNpgSqlMetrics("Users", configuration.GetConnectionString("Users"));

            services.AddDbContext<ClassicServersContext>(options => options.UseNpgsql(configuration.GetConnectionString("ClassicServers")));
            services.AddTransient<IClassicServersRepository, EfClassicServersRepository>();

            services.AddHostedService<ClassicServersMonitor>();

            services.AddTransient<IEmailSender, AuthMessageSender>();

            //services.AddHostedService<MetricsHttpListenerMonitor>();

            services.AddDbContext<UserContext>(options => options
                .ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
                .UseNpgsql(configuration.GetConnectionString("Users")));

            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UserContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            ConfigureAuthentication(context, configuration);


            /*
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                foreach (var address in Dns.GetHostEntry("minelib.server.website.nginx").AddressList)
                    options.KnownProxies.Add(address);
            });


            services.AddMvc();


            */

            /*
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@._+";
                options.User.RequireUniqueEmail = false;
            });
            */

            //services.AddControllersWithViews();
            //services.AddControllers();
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            //app.UseJwtTokenMiddleware();
            //app.UseIdentityServer();
            app.UseAuthorization();

            app.UseMvcWithDefaultRouteAndArea();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute();
            //    endpoints.MapRazorPages();
            //});
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            var serviceOptions = context.ServiceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;

            try
            {
                using var scope = context.ServiceProvider.CreateScope();
                var services = scope.ServiceProvider;
                var classicServersContext = services.GetRequiredService<ClassicServersContext>();
                classicServersContext.Database.EnsureDeleted();
                classicServersContext.Database.EnsureCreated();

                var userContext = services.GetRequiredService<UserContext>();
                //userContext.Database.EnsureDeleted();
                userContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var logger = context.ServiceProvider.GetRequiredService<ILogger<WebSiteModule>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }
}