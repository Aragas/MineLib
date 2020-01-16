using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Aragas.QServer.Core
{
    public class BaseHostProgram
    {
        public static async Task Main<TProgram>(Func<IHostBuilder> hostBuilder) where TProgram : BaseHostProgram
        {
            Aragas.Network.Extensions.PacketExtensions.Init();
            //MineLib.Core.Extensions.PacketExtensions.Init();
            //MineLib.Server.Core.Extensions.PacketExtensions.Init();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("{TypeName}: Starting.", typeof(TProgram).Name);
                //using var program = new TProgram();

                await hostBuilder()
                    .UseSerilog()
                    .Build()
                    .RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{TypeName}: Fatal exception.", typeof(TProgram).Name);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}