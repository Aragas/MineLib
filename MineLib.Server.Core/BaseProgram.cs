using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.Core
{
    public class BaseProgram : IDisposable
    {
        public static TimeSpan RestartAfter { get; } = TimeSpan.FromSeconds(10);
        public static CancellationTokenSource GlobalCancellationTokenSource { get; } = new CancellationTokenSource();

        public static async Task Main<TProgram>(string[] args) where TProgram : BaseProgram, new()
        {
            Aragas.Network.Extensions.PacketExtensions.Init();
            MineLib.Core.Extensions.PacketExtensions.Init();
            MineLib.Server.Core.Extensions.PacketExtensions.Init();

            AppDomain.CurrentDomain.UnhandledException += HandleException;

            TProgram? program = default;
            DateTime lastRunTime = default;
        Start:
            try
            {
                if (GlobalCancellationTokenSource.IsCancellationRequested) return;
                lastRunTime = DateTime.UtcNow;
                program = new TProgram();
                await program.RunAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e);

                if (DateTime.UtcNow - lastRunTime > RestartAfter)
                {
                    program?.Dispose();
                    goto Start;
                }
                else
                {
                    Environment.Exit(1);
                }
            }
            finally
            {
                GlobalCancellationTokenSource.Dispose();
                program?.Dispose();
            }

            AppDomain.CurrentDomain.UnhandledException -= HandleException;
        }

        private static void HandleException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception ?? new NotSupportedException("Unhandled exception doesn't derive from System.Exception: " + e.ExceptionObject);
            HandleException(exception);
        }
        private static void HandleException(Exception e)
        {
            var exceptionText = ProcessException(e);
            ReportExceptionLocal(exceptionText);
        }

        private static string ProcessException(Exception ex)
        {
            var platformService = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default;

            return
                $@"[CODE]
{Assembly.GetEntryAssembly().GetName().Name} Crash Log v{Assembly.GetEntryAssembly().GetName().Version}
Software:
    OS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription} {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture} [{platformService.Application.RuntimeFramework.FullName}]
    Language: {CultureInfo.CurrentCulture.EnglishName}
    Framework: 
        Runtime {typeof(System.Runtime.InteropServices.RuntimeInformation).Assembly.GetCustomAttributes().OfType<AssemblyInformationalVersionAttribute>().Single().InformationalVersion}
{RecursiveException(ex)}
You should report this error if it is reproduceable or you could not solve it by yourself.
[/CODE]";
        }
        private static string RecursiveException(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                $@"Exception information:
Type: {ex.GetType().FullName}
Message: {ex.Message}
HelpLink: {(string.IsNullOrWhiteSpace(ex.HelpLink) ? "Empty" : ex.HelpLink)}
Source: {ex.Source}
TargetSite : {ex.TargetSite}
CallStack:
{ex.StackTrace}");

            if (ex.InnerException != null)
            {
                sb.AppendFormat($@"
--------------------------------------------------
InnerException:
{RecursiveException(ex.InnerException)}");
            }

            return sb.ToString();
        }

        private static void ReportExceptionLocal(string exception)
        {
            using var stream = File.OpenWrite(Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}.log"));
            using var writer = new StreamWriter(stream);
            writer.Write(exception);
        }


        public static CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public virtual Task RunAsync() { Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US"); return Task.CompletedTask; }
        public virtual Task StopAsync()  { CancellationTokenSource.Cancel(); return Task.CompletedTask;}


        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CancellationTokenSource.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}