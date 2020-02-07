using App.Metrics.Health;

using Aragas.QServer.Health;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MineLib.Server.WebSite.BackgroundServices;
using MineLib.Server.WebSite.Data;
using MineLib.Server.WebSite.Extensions;
using MineLib.Server.WebSite.Models;
using MineLib.Server.WebSite.Repositories;
using MineLib.Server.WebSite.Services;

using Serilog;

using System;
using System.Net;
using System.Threading.Tasks;

namespace MineLib.Server.WebSite
{
    public sealed class Program
    {
        private static Guid Uid { get; } = Guid.NewGuid();

        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("loggerconfig.json").Build();
            Log.Logger = new LoggerConfiguration()
                .ConfigureSerilog(Uid)
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("{TypeName}: Starting.", typeof(Program).FullName);

                var hostBuilder = CreateHostBuilder(args ?? Array.Empty<string>());

                var host = hostBuilder.Build();

                BeforeRun(host.Services);

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{TypeName}: Fatal exception.", typeof(Program).FullName);
                throw;
            }
            finally
            {
                Log.Information("{TypeName}: Stopped.", typeof(Program).FullName);
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args ?? Array.Empty<string>())
            .ConfigureLogging(logging =>
            {
                logging.AddSerilog(dispose: false);
#if DEBUG
                logging.AddDebug();
#endif
            })

            // Metrics
            .ConfigureServices((hostContext, services) =>
            {
                services.AddPrometheusEndpoint();
                services.AddDefaultMetrics();

                services.AddNpgSqlMetrics("ClassicServers", hostContext.Configuration.GetConnectionString("ClassicServers"));
                services.AddNpgSqlMetrics("Users", hostContext.Configuration.GetConnectionString("Users"));
            })
            // HealthCheck
            .ConfigureServices(services =>
            {
                services.AddSingleton<HealthCheck, CpuHealthCheck>();
                services.AddSingleton<HealthCheck, RamHealthCheck>();
                services.AddHealthCheckPublisher();
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

                        services.AddMvc()
                            .AddMetrics();

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

        private static void BeforeRun(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context0 = services.GetRequiredService<ClassicServersContext>();
                context0.Database.EnsureDeleted();
                context0.Database.EnsureCreated();

                var context1 = services.GetRequiredService<UserContext>();
                //context1.Database.EnsureDeleted();
                context1.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }

        }
    }
}