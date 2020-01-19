using Aragas.Network.IO;

namespace PokeD.Core.Packets.SCON.Script
{
    public class UploadScriptToServerPacket : SCONPacket
    {
        public string ScriptFile { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            ScriptFile = deserializer.Read(ScriptFile);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ScriptFile);
        }
    }
}
