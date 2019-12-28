using Aragas.QServer.Core;

using System.Threading.Tasks;

namespace PokeD.Server.Core
{
    public class PokeDProgram : BaseProgram
    {
        public static async new Task Main<TProgram>(string[] args) where TProgram : BaseProgram, new()
        {
            PokeD.Server.Core.Extensions.PacketExtensions.Init();
            //MineLib.Server.Core.Extensions.PacketExtensions.Init();

            await BaseProgram.Main<TProgram>(args);
        }
    }
}