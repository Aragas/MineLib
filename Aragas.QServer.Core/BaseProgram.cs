using Aragas.QServer.Core.Storage.Files;

using Firebase.Storage;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Core
{
    public partial class BaseProgram : IDisposable
    {
        public static TimeSpan RestartAfter { get; protected set; } = TimeSpan.FromSeconds(10);
        public static CancellationTokenSource GlobalCancellationTokenSource { get; } = new CancellationTokenSource();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1163:Unused parameter.", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static async Task Main<TProgram>(string[] args) where TProgram : BaseProgram, new()
        {
            Aragas.Network.Extensions.PacketExtensions.Init();
            //MineLib.Core.Extensions.PacketExtensions.Init();
            //MineLib.Server.Core.Extensions.PacketExtensions.Init();

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
            catch (Exception e) when(e is Exception)
            {
                HandleException(e);

#if DEBUG
                throw;
#endif

                if (DateTime.UtcNow - lastRunTime > RestartAfter)
                {
                    program?.Dispose();
                    goto Start;
                }
                else
                {
                    Environment.Exit(1);
                    return;
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
            var exception = e.ExceptionObject as Exception ?? new NotSupportedException($"Unhandled exception doesn't derive from System.Exception: {e.ExceptionObject}");
            HandleException(exception);
        }
        private static void HandleException(Exception e)
        {
            var exceptionText = ProcessException(e);
            ReportExceptionLocal(exceptionText);
            ReportErrorWeb(exceptionText);
        }

        private static string ProcessException(Exception ex)
        {
            var osInfo = SystemInfoLibrary.OperatingSystem.OperatingSystemInfo.GetOperatingSystemInfo();
            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            return $@"{assemblyName.Name} Crash Log v {assemblyName.Version}
Software:
    OS: {osInfo.Name} {osInfo.Architecture}
    Language: {CultureInfo.CurrentCulture.EnglishName}
    Framework:
        CompiledWith: {Assembly.GetEntryAssembly()?.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>()?.FrameworkName ?? "UNKNOWN"}
        Runtime: {osInfo.Runtime}
Hardware:
{RecursiveCPU(osInfo.Hardware.CPUs, 0)}
{RecursiveGPU(osInfo.Hardware.GPUs, 0)}
    RAM:
        Memory Total: {osInfo.Hardware.RAM.Total} KB
        Memory Free: {osInfo.Hardware.RAM.Free} KB
{RecursiveException(ex)}
You should report this error if it is reproduceable or you could not solve it by yourself.
Go To: REPORTURL to report this crash there.";
        }
        private static string RecursiveException(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendFormat($@"Exception information:
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
        private static string RecursiveCPU(IList<SystemInfoLibrary.Hardware.CPU.CPUInfo> cpus, int index)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                $@"    CPU{index}:
        Name: {cpus[index].Name}
        Brand: {cpus[index].Brand}
        Architecture: {cpus[index].Architecture}
        Physical Cores: {cpus[index].PhysicalCores}
        Logical Cores: {cpus[index].LogicalCores}");

            if (index + 1 < cpus.Count)
                sb.AppendFormat(RecursiveCPU(cpus, ++index));

            return sb.ToString();
        }
        private static string RecursiveGPU(IList<SystemInfoLibrary.Hardware.GPU.GPUInfo> gpus, int index)
        {
            var sb = new StringBuilder();
            sb.AppendFormat($@"    GPU{index}:
        Name: {gpus[index].Name}
        Brand: {gpus[index].Brand}
        Memory Total: {gpus[index].MemoryTotal} KB");

            if (index + 1 < gpus.Count)
                sb.AppendFormat(RecursiveGPU(gpus, ++index));

            return sb.ToString();
        }

        private static void ReportExceptionLocal(string exception)
        {
            using var stream = new CrashLogFile().Open(PCLExt.FileStorage.FileAccess.ReadAndWrite);
            using var writer = new StreamWriter(stream);
            writer.Write(exception);
        }
        private static void ReportErrorWeb(string exception)
        {
            return;
            try
            {
                using var cts = new CancellationTokenSource(2000);
                using var ms = new MemoryStream(Encoding.UTF8.GetBytes(exception));
                // TODO
                new FirebaseStorage("factride-66653.appspot.com")
                    .Child($"CrashLogs")
                    .Child($"{Assembly.GetExecutingAssembly().GetName().Name} {Assembly.GetExecutingAssembly().GetName().Version}")
                    .Child($"{DateTime.UtcNow:yyyy-MM-dd_HH.mm.ss}.log")
                    .PutAsync(ms, cts.Token, "text/plain").GetAwaiter().GetResult();
            }
            catch (Exception e) when (e is FirebaseStorageException) { }
        }


        protected CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        protected Guid ProgramGuid { get; } = Guid.NewGuid();

        public virtual Task RunAsync()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            return Task.CompletedTask;
        }
        public virtual Task StopAsync()
        {
            CancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }


        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CancellationTokenSource.Dispose();
                    AppMetricsPrometheusEvent.Dispose();
                    AppMetricsHealthEvent.Dispose();
                    AppDomain.CurrentDomain.FirstChanceException -= CurrentDomain_FirstChanceException;
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