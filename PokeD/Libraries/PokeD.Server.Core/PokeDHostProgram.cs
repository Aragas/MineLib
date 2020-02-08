using Aragas.QServer.Hosting;

using Microsoft.Extensions.Hosting;

using System;
using System.Threading.Tasks;

namespace PokeD.Server.Core
{
    public static class PokeDHostProgram
    {
        public static async new Task Main<TProgram>(Func<IHostBuilder, IHostBuilder>? hostBuilderFunc = null, Action<IHost>? beforeRunAction = null, string[]? args = null)
        {
            PokeD.Core.Extensions.PacketExtensions.Init();
            PokeD.Server.Core.Extensions.PacketExtensions.Init();

            await QServerHostProgram.Main<TProgram>(hostBuilderFunc, beforeRunAction, args);
        }
    }
}