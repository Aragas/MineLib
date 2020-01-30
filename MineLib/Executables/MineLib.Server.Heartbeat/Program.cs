using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MineLib.Server.Heartbeat.Infrastructure.Data;
using MineLib.Server.Heartbeat.Models;
using MineLib.Server.Heartbeat.Services;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat
{
    public class ApplicationInfoEnricher : ILogEventEnricher
    {
        public const string ApplicationPropertyName = "Application";
        public const string ApplicationUidPropertyName = "ApplicationUid";

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationProperty(ILogEventPropertyFactory propertyFactory) =>
            propertyFactory.CreateProperty(ApplicationPropertyName, Assembly.GetEntryAssembly()?.GetName().Name ?? "ERROR");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationUidProperty(ILogEventPropertyFactory propertyFactory, Guid applicationUid) =>
            propertyFactory.CreateProperty(ApplicationUidPropertyName, applicationUid);

        private readonly Guid _applicationUid;

        private LogEventProperty _cachedApplicationNameProperty;
        private LogEventProperty _cachedApplicationUidProperty;

        public ApplicationInfoEnricher(Guid applicationUid)
        {
            _applicationUid = applicationUid;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetApplicationLogEventProperty(propertyFactory));
            logEvent.AddPropertyIfAbsent(GetApplicationUidLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetApplicationLogEventProperty(ILogEventPropertyFactory propertyFactory) =>
            _cachedApplicationNameProperty ?? (_cachedApplicationNameProperty = CreateApplicationProperty(propertyFactory));

        private LogEventProperty GetApplicationUidLogEventProperty(ILogEventPropertyFactory propertyFactory) =>
            _cachedApplicationUidProperty ?? (_cachedApplicationUidProperty = CreateApplicationUidProperty(propertyFactory, _applicationUid));
    }
    public class LogLevelEnricher : ILogEventEnricher
    {
        public const string LevelPropertyName = "Level";

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateLevelProperty(ILogEventPropertyFactory propertyFactory, LogEventLevel level) =>
            propertyFactory.CreateProperty(LevelPropertyName, level.ToString());

        private Dictionary<LogEventLevel, LogEventProperty> _cachedLevelProperty = new Dictionary<LogEventLevel, LogEventProperty>();

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLevelLogEventProperty(propertyFactory, logEvent.Level));
        }

        private LogEventProperty GetLevelLogEventProperty(ILogEventPropertyFactory propertyFactory, LogEventLevel level)
        {
            if (!_cachedLevelProperty.ContainsKey(level))
                _cachedLevelProperty[level] = CreateLevelProperty(propertyFactory, level);

            return _cachedLevelProperty[level];
        }
    }

    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration ConfigureSerilog(this LoggerConfiguration loggerConfiguration, Guid applicationUid)
        {
            return loggerConfiguration
                .Enrich.WithExceptionDetails()
                .Enrich.WithApplicationInfo(applicationUid)
                .Enrich.WithLogLevel()
                ;
        }

        public static LoggerConfiguration WithApplicationInfo(this LoggerEnrichmentConfiguration enrichmentConfiguration, Guid applicationUid)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new ApplicationInfoEnricher(applicationUid));
        }
        public static LoggerConfiguration WithLogLevel(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<LogLevelEnricher>();
        }
    }

    public class Program
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
            catch (Exception ex) when (ex is Exception)
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
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<ClassicServersContext>(options => options.UseNpgsql(hostContext.Configuration.GetConnectionString("ClassicServers")));
                services.AddTransient<IClassicServersRepository, EfClassicServersRepository>();

                services.AddTransient<IEmailSender, AuthMessageSender>();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureServices((hostContext, services) =>
                    {
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
                        .AddIdentityCookies(o => { });

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

                        //app.UseHttpsRedirection();
                        app.UseStaticFiles();

                        app.UseRouting();

                        app.UseAuthentication();
                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapDefaultControllerRoute();
                            endpoints.MapRazorPages();

                            /*
                            endpoints.MapControllers();
                            endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}");
                            */
                        });
                    })
                    .UseKestrel();
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