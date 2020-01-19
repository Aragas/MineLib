using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class SteerVehiclePacket : ServerPlayPacket
    {
        public Single Sideways;
        public Single Forward;
        public Boolean Jump;
        public Boolean Unmount;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Sideways = deserializer.Read(Sideways);
            Forward = deserializer.Read(Forward);
            Jump = deserializer.Read(Jump);
            Unmount = deserializer.Read(Unmount);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Sideways);
            serializer.Write(Forward);
            serializer.Write(Jump);
            serializer.Write(Unmount);
        }
    }
}