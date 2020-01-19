using Aragas.Network.IO;

namespace PokeD.Core.Packets.SCON
{
    public class ExecuteCommandPacket : SCONPacket
    {
        public string Command { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Command = deserializer.Read(Command);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Command);
        }
    }
}
