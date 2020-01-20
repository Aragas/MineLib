using Aragas.QServer.Core;

using Microsoft.Extensions.Hosting;

using System;
using System.Threading.Tasks;

namespace MineLib.Server.Core
{
    public class MineLibHostProgram : BaseHostProgram
    {
        public static async new Task Main<TProgram>(Func<IHostBuilder, IHostBuilder>? hostBuilderFunc = null, Action<IServiceProvider>? beforeRunAction = null, string[]? args = null)
            where TProgram : MineLibHostProgram
        {
            MineLib.Core.Extensions.PacketExtensions.Init();
            MineLib.Server.Core.Extensions.PacketExtensions.Init();

            await BaseHostProgram.Main<TProgram>(hostBuilderFunc, beforeRunAction, args);
        }
    }
}