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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
            Sideways = deserialiser.Read(Sideways);
            Forward = deserialiser.Read(Forward);
            Jump = deserialiser.Read(Jump);
            Unmount = deserialiser.Read(Unmount);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Sideways);
            serializer.Write(Forward);
            serializer.Write(Jump);
            serializer.Write(Unmount);
        }
    }
}