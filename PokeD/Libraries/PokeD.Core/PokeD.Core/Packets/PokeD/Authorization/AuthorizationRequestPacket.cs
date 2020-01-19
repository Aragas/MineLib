﻿using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Authorization
{
    public class AuthorizationRequestPacket : PokeDPacket
    {
        public string Name { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Name = deserializer.Read(Name);
        }
        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Name);
        }
    }
}
