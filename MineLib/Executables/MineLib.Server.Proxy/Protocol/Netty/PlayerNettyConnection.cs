using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus.Messages;

using MineLib.Server.Core.NetworkBus.Messages;
using MineLib.Server.Proxy.Packets.Netty;
using MineLib.Server.Proxy.Packets.Netty.Clientbound;
using MineLib.Server.Proxy.Packets.Netty.Serverbound;
using MineLib.Server.Proxy.Translation;

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.Proxy.Protocol.Netty
{
    internal sealed class PlayerNettyConnection : DefaultConnectionHandler<ProxyNettyTransmission, ProxyNettyPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        private static string GetJSONResponse() => @$"
{{
    ""version"":
    {{
        ""name"": ""Any Version"",
        ""protocol"": 0
    }},
    ""players"":
    {{
        ""max"": {Program.MaxConnections},
        ""online"": {Program.CurrentConnections}
    }},	
    ""description"": 
    {{
        ""text"": ""{Program.Description}""
    }},
    ""favicon"": ""{GetFavicon()}"",
    ""modinfo"":
    {{
        ""type"": ""FML"",
        ""modList"": []
    }}
}}
";
        private static string GetFavicon()
        {
            using var ms = new MemoryStream();
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MineLib.Server.Proxy.logo-1.png");
            if (stream != null)
            {
                stream.CopyTo(ms);
                var png = ms.ToArray();
                var base64Png = Convert.ToBase64String(png);
                return $"data:image/png;base64,{base64Png}";
            }
            return string.Empty;
        }

        private Guid? PlayerBusId { get; set; }

        private Guid PlayerId { get; } = Guid.NewGuid();

        private ConcurrentQueue<byte[]> DataToSend { get; } = new ConcurrentQueue<byte[]>();

        private IDisposable PlayerBusDataEvent { get; set; }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public PlayerNettyConnection() { }
        public PlayerNettyConnection(Socket socket) : base(socket) { }

        protected override void HandlePacket(ProxyNettyPacket packet)
        {
            switch (packet)
            {
                case RequestPacket _:
                    SendPacket(new ResponsePacket() { JSONResponse = GetJSONResponse() });
                    break;

                case PingPacket pingPacket:
                    SendPacket(new PongPacket() { Time = pingPacket.Time });
                    Task.Run(Disconnect);
                    break;
            }
        }

        private bool HasInit = false;
        protected override void AdditionalWork()
        {
            if(Stream != null && !HasInit)
            {
                HasInit = true;

                Stream.OnDataReceived += data => DataToSend.Enqueue(data);
            }

            if (Stream.State == Data.State.Handshake || Stream.State == Data.State.Status)
                return;

            if (PlayerBusId == null)
            {
                Guid? playerBusId = null;

                var awaiter = new ManualResetEvent(true);
                using var _ = BaseSingleton.Instance.Subscribe<GetExistingPlayerHandlerResponseMessage>(message =>
                {
                    if (message.ServiceId != null)
                    {
                        playerBusId = message.ServiceId;
                        Stream.State = (Data.State) message.State;
                    }
                });
                BaseSingleton.Instance.Publish(new GetExistingPlayerHandlerRequestMessage()
                {
                    PlayerId = PlayerId,
                    ProtocolVersion = Stream.ProtocolVersion
                });
                awaiter.WaitOne(2000);

                if (playerBusId == null)
                {
                    var response = BaseSingleton.Instance.PublishAndWaitForExclusiveResponse<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>(
                        new GetNewPlayerHandlerRequestMessage()
                        {
                            PlayerId = PlayerId,
                            ProtocolVersion = Stream.ProtocolVersion
                        });
                    if (response != null)
                        playerBusId = response.ServiceId;
                }


                if (playerBusId == null)
                {
                    if (Stream.State == Data.State.Login)
                    {
                        SendPacket(new Disconnect2Packet()
                        {
                            JSONData = $@"{{ ""text"": ""{Strings.LoginKicked}"" }}"
                        });
                    }
                    Disconnect();
                }
                else
                {
                    PlayerBusId = playerBusId;
                    PlayerBusDataEvent = BaseSingleton.Instance.Subscribe<PlayerDataToProxyMessage>(message =>
                    {
                        try
                        {
                            Stream.Socket.Send(message.Data);
                        }
                        catch (Exception e) when (e is SocketException)
                        {
                            PlayerBusDataEvent.Dispose();
                        }
                    }, PlayerId);
                }
            }
            else
            {
                while (DataToSend.TryDequeue(out var data))
                    BaseSingleton.Instance.Publish(new PlayerDataToBusMessage() { Data = data }, PlayerId);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PlayerBusDataEvent?.Dispose();
                DataToSend.Clear();
            }

            base.Dispose(disposing);
        }
    }
}