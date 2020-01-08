using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Aragas.Network.Data;
using Aragas.Network.Extensions;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PokeD.Core.Data.PokeApi;
using PokeD.Core.Extensions;
using PokeD.Core.IO;
using PokeD.Core.Packets.P3D;
using PokeD.Core.Packets.PokeD;
using PokeD.Core.Packets.SCON;
using PokeD.Core.Test;


namespace PokeD.Core.Test
{
    [TestClass]
    public class PacketTest
    {
        public PacketTest()
        {
            PokeApiV2.CacheType = PokeApiV2.CacheTypeEnum.None;

            Extensions.PacketExtensions.Init();
        }


        /*
        [TestMethod]
        public void TestP3DProtobufSerializationDeserialization()
        {
            var packets = P3DPacketResponses.Packets.Where(packetFunc => packetFunc != null).Select(packetFunc => packetFunc()).ToList();
            var testPackets = new List<P3DPacket>();

            var tcpClient = new TestITCPClient();
            using (var write = new P3DStream(tcpClient))
            {
                foreach (var packet in packets)
                    write.SendPacket(packet);


                tcpClient.Stream.Seek(0, SeekOrigin.Begin);


                string data;
                while (!string.IsNullOrEmpty((data = write.ReadLine())))
                    testPackets.Add(GetPacket(data));

                Assert.IsTrue(packets.Count == testPackets.Count);
                for (var i = 0; i < packets.Count; i++)
                    Assert.IsTrue(packets[i].ID == testPackets[i].ID);
            }
        }
        private static P3DPacket GetPacket(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                int id;
                if (P3DPacket.TryParseID(data, out id))
                {
                    Func<P3DPacket> func;
                    if (P3DPacketResponses.TryGetPacketFunc(id, out func))
                    {
                        if (func != null)
                        {
                            var packet = func();
                            if (packet.TryParseData(data))
                                return packet;
                            else
                                AssertEx.Fail();
                        }
                        else
                            AssertEx.Fail();
                    }
                    else
                        AssertEx.Fail();
                }
                else
                    AssertEx.Fail();
            }
            else
                AssertEx.Fail();

            return null;
        }
        */


        [TestMethod]
        public void TestPokeDProtobufSerializationDeserialization() { /* SerializationDeserialization(PokeDPacketResponses.Packets); */ }

        [TestMethod]
        public void TestSCONProtobufSerializationDeserialization() { /* SerializationDeserialization(SCONPacketResponses.Packets); */ }
        
        private void SerializationDeserialization<TPacketType>(Func<TPacketType>[] packetFuncs) where TPacketType : PacketWithIntegerType<VarInt, ProtobufSerializer, ProtobufDeserialiser>
        {
            var packets = packetFuncs.Where(packetFunc => packetFunc != null).Select(packetFunc => packetFunc()).ToList();
            var testPackets = new List<TPacketType>();

            var tcpClient = new TestITCPClient();
            using (var transmission = new ProtobufTransmission<TPacketType>(tcpClient))
            {
                foreach (var packet in packets)
                    transmission.SendPacket(packet);


                tcpClient.Stream.Seek(0, SeekOrigin.Begin);


                while (tcpClient.Stream.Position != tcpClient.Stream.Length)
                {
                    testPackets.Add(transmission.ReadPacket());
                    //int length = transmission.ReadVarInt();
                    //var byteArray = transmission.Receive(length);
                    //testPackets.Add((TPacketType) GetPacket(byteArray, packetFuncs));
                }

                Assert.IsTrue(packets.Count == testPackets.Count);
                for (var i = 0; i < packets.Count; i++)
                    Assert.IsTrue(packets[i].ID == testPackets[i].ID);
            }
        }
        private static PacketWithIntegerType<VarInt, ProtobufSerializer, ProtobufDeserialiser> GetPacket<TPacketType>(byte[] byteArray, Func<TPacketType>[] packetFuncs) where TPacketType : PacketWithIntegerType<VarInt, ProtobufSerializer, ProtobufDeserialiser>
        {
            using (var deserialiser = new ProtobufDeserialiser(byteArray))
            {
                var id = deserialiser.Read<VarInt>();

                if (packetFuncs.Length > id)
                {
                    if (packetFuncs[id] != null)
                    {
                        var packet = packetFuncs[id]();
                        packet.Deserialize(deserialiser);
                        return packet;
                    }
                    else
                        Assert.Fail();
                }
                else
                    Assert.Fail();
            }

            return null;
        }
    }
}