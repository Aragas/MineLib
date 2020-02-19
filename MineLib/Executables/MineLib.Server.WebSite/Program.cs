using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
using System.Net;
using System.Threading.Tasks;

namespace MineLib.Server.WebSite
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            await QServerHostProgram.Main<Program>(CreateHostBuilder, BeforeRun, args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "WebSite");
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddNpgSqlMetrics("ClassicServers", hostContext.Configuration.GetConnectionString("ClassicServers"));
                services.AddNpgSqlMetrics("Users", hostContext.Configuration.GetConnectionString("Users"));
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<ClassicServersContext>(options => options.UseNpgsql(hostContext.Configuration.GetConnectionString("ClassicServers")));
                services.AddTransient<IClassicServersRepository, EfClassicServersRepository>();

                services.AddHostedService<ClassicServersMonitor>();

                services.AddTransient<IEmailSender, AuthMessageSender>();

                services.AddHostedService<MetricsHttpListenerMonitor>();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.Configure<ForwardedHeadersOptions>(options =>
                        {
                            foreach (var address in Dns.GetHostEntry("minelib.server.website.nginx").AddressList)
                                options.KnownProxies.Add(address);
                        });

                        services.AddDbContext<UserContext>(options => options
                            .ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
                            .UseNpgsql(hostContext.Configuration.GetConnectionString("Users")));

                        services.AddMvc();

                        services.AddIdentityCore<User>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<UserContext>()
                            .AddSignInManager()
                            .AddDefaultTokenProviders();

                        services.AddAuthentication(o =>
                        {
                            o.DefaultScheme = IdentityConstants.ApplicationScheme;
                            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                        })
                        .AddIdentityCookies();

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
                    })
                    .Configure(app =>
                    {
                        /*
                        if (env.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                            app.UseDatabaseErrorPage();
                        }
                        else
                        {
                            app.UseExceptionHandler("/Home/Error");
                        }
                        */

                        app.UseForwardedHeaders(new ForwardedHeadersOptions
                        {
                            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                        });

                        //app.UseHttpsRedirection();
                        app.UseStaticFiles();

                        app.UseRouting();

                        app.UseAuthentication();
                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapDefaultControllerRoute();
                            endpoints.MapRazorPages();
                        });
                    })
                    .UseKestrel(o => o.AllowSynchronousIO = true);
            });

        private static void BeforeRun(IHost host)
        {
            var serviceOptions = host.Services.GetRequiredService<IOptions<ServiceOptions>>().Value;

            try
            {
                using var scope = host.Services.CreateScope();
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
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }
}