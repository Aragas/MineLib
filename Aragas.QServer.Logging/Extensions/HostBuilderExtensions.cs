using Microsoft.Extensions.Logging;

using Serilog;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseSerilog(this IHostBuilder hostBuilder) =>
            hostBuilder.ConfigureLogging((hostContext, logging) =>
            {
                logging.AddSerilog(dispose: false);
#if DEBUG
                logging.AddDebug();
#endif
            });
    }
}