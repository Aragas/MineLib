using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Aragas.Core.Data;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using MineLib.Core.Client;
using MineLib.Core.Events;

using Newtonsoft.Json;

using ProtocolModern.Data;
using ProtocolModern.Enum;
using ProtocolModern.IO;
using ProtocolModern.Packets.Client.Status;
using ProtocolModern.Packets.Server.Handshake;
using ProtocolModern.Packets.Server.Status;

namespace ProtocolModern
{
    public sealed class StatusClient : IStatusClient
    {
        private sealed class HandshakeEvent : SendingEvent
        {
            public string IP { get; }
            public ushort Port { get; }

            public int ProtocolVersion { get; }

            public HandshakeEvent(string ip, ushort port, int protocolVersion)
            {
                IP = ip;
                Port = port;

                ProtocolVersion = protocolVersion;
            }
        }
        private sealed class SendRequestEvent : SendingEvent { }

        
        private event Action<ProtobufPacket> OnResponsePacket;


        public ServerInfo GetServerInfo(string ip, ushort port, int protocolVersion)
        {
            var responseData = new ServerResponse();
            var response = false;

            var protocol = new Client.Protocol(null, ProtocolPurpose.InfoRequest);
            protocol.Connect(new ServerInfo { Address = new ServerAddress { IP = ip, Port = port } });

            protocol.RegisterCustomReceiving<ResponsePacket>(ReceiveResponse);
            OnResponsePacket += packet => { responseData.Info = ParseResponse((ResponsePacket) packet); response = true; };

            protocol.RegisterSending<HandshakeEvent>(SendHandshake);
            protocol.RegisterSending<SendRequestEvent>(SendSendRequest);

            protocol.FireEvent(new HandshakeEvent(ip, port, protocolVersion));
            protocol.FireEvent(new SendRequestEvent());
            
            var watch = Stopwatch.StartNew();
            while (!response) { Task.Delay(100).Wait(); if(watch.ElapsedMilliseconds > 2000) { responseData.Ping = long.MaxValue;  break;} }

            protocol.Disconnect();
            protocol.Dispose();

            if(response)
                responseData.Ping = PingServer(ip, port);

            return new ServerInfo
            {
                Address = new ServerAddress { IP = ip, Port = port },
                ServerResponse = responseData
            };
        }

        public long GetPing(string ip, ushort port) { return PingServer(ip, port); }


        private static void SendHandshake(HandshakeEvent args) { args.SendPacket?.Invoke(new HandshakePacket { ServerAddress = args.IP, ServerPort = args.Port, ProtocolVersion = new VarInt(args.ProtocolVersion), NextState = NextState.Status }); }
        private static void SendSendRequest(SendRequestEvent args) { args.SendPacket?.Invoke(new RequestPacket()); }

        private void ReceiveResponse(ResponsePacket packet) { OnResponsePacket?.Invoke(packet); }


        private static ServerData ParseResponse(ResponsePacket packet) { return JsonConvert.DeserializeObject<ServerData>(packet.JSONResponse, new Base64JsonConverter()); }

        private static long PingServer(string host, ushort port)
        {
            var watch = Stopwatch.StartNew();
            var client = TCPClientWrapper.Create();
            client.Connect(host, port);
            client.Disconnect();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
    }
}
