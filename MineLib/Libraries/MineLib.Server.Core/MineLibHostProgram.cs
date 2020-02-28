using Aragas.QServer.Hosting;

using Microsoft.Extensions.Hosting;

using System;
using System.Threading.Tasks;

namespace MineLib.Server.Core
{
    public static class MineLibHostProgram
    {
        public static async new Task Main<TProgram>(Func<IHostBuilder, IHostBuilder>? hostBuilderFunc = null, Action<IHost>? beforeRunAction = null, string[]? args = null)
        {
            MineLib.Core.Extensions.PacketExtensions.Init();
            MineLib.Server.Core.Extensions.PacketExtensions.Init();

            await QServerHostProgram.Main<TProgram>(hostBuilderFunc: hostBuilderFunc, beforeRunAction: beforeRunAction, args: args);
        }
    }
}