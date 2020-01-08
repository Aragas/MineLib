using App.Metrics;
using App.Metrics.Health;

using Aragas.QServer.Core;

using System;
using System.Threading.Tasks;

namespace PokeD.Server.Core
{
    public class PokeDProgram : BaseProgram
    {
        public PokeDProgram(Func<IMetricsBuilder, IMetricsBuilder>? metricsConfigure = null, Func<IHealthBuilder, IHealthBuilder>? healthConfigure = null)
            : base(metricsConfigure, healthConfigure) { }

        public static async new Task Main<TProgram>(string[] args) where TProgram : BaseProgram, new()
        {
            PokeD.Server.Core.Extensions.PacketExtensions.Init();
            PokeD.Core.Extensions.PacketExtensions.Init();

            await BaseProgram.Main<TProgram>(args);
        }
    }
}