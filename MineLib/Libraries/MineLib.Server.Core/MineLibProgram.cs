using App.Metrics;
using App.Metrics.Health;
using Aragas.QServer.Core;
using System;
using System.Threading.Tasks;

namespace MineLib.Server.Core
{
    public class MineLibProgram : BaseProgram
    {
        public MineLibProgram(Func<IMetricsBuilder, IMetricsBuilder>? metricsConfigure = null, Func<IHealthBuilder, IHealthBuilder>? healthConfigure = null)
            : base(metricsConfigure, healthConfigure) { }

        public static async new Task Main<TProgram>(string[] args) where TProgram : BaseProgram, new()
        {
            MineLib.Core.Extensions.PacketExtensions.Init();
            MineLib.Server.Core.Extensions.PacketExtensions.Init();

            await BaseProgram.Main<TProgram>(args);
        }
    }
}