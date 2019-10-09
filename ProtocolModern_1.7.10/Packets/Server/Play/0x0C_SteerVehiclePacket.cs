using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class SteerVehiclePacket : ProtobufPacket
    {
        public Single Sideways;
        public Single Forward;
        public Boolean Jump;
        public Boolean Unmount;

        public override VarInt ID => ServerPlayPacketTypes.SteerVehicle;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
            Sideways = reader.Read(Sideways);
            Forward = reader.Read(Forward);
            Jump = reader.Read(Jump);
            Unmount = reader.Read(Unmount);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            stream.Write(Sideways);
            stream.Write(Forward);
            stream.Write(Jump);
            stream.Write(Unmount);

            return this;
        }

    }
}