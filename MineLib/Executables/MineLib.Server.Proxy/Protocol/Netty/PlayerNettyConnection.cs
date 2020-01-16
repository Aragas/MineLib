using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;

using Microsoft.Extensions.Localization;

using MineLib.Server.Core.NetworkBus.Messages;
using MineLib.Server.Proxy.Data;
using MineLib.Server.Proxy.Protocol.Netty.Packets;
using MineLib.Server.Proxy.Protocol.Netty.Packets.ClientBound;
using MineLib.Server.Proxy.Protocol.Netty.Packets.Serverbound;

using System;
using System.IO;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.Proxy.Protocol.Netty
{
    internal sealed class PlayerNettyConnection :
        DefaultConnectionHandler<ProxyNettyTransmission, ProxyNettyPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
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
        private string GetJSONResponse() => @$"
{{
    ""version"":
    {{
        ""name"": ""Any Version"",
        ""protocol"": 0
    }},
    ""players"":
    {{
        ""max"": {ServerInfo.MaxConnections},
        ""online"": {ServerInfo.CurrentConnections}
    }},
    ""name"":
    {{
        ""text"": ""{ServerInfo.Name}""
    }},
    ""description"":
    {{
        ""text"": ""{ServerInfo.Description}""
    }},
    ""favicon"": ""{GetFavicon()}"",
    ""modinfo"":
    {{
        ""type"": ""FML"",
        ""modList"": []
    }}
}}
";

        private Guid? PlayerBusId { get; set; }

        private Guid PlayerId { get; } = Guid.NewGuid();

        private CompositeDisposable Events { get; } = new CompositeDisposable();
        private INetworkBus NetworkBus { get; }
        private ServerInfo ServerInfo { get; }
        private IStringLocalizer Localizer { get; }

        public PlayerNettyConnection(ServerInfo serverInfo, Socket socket, INetworkBus networkBus, IStringLocalizer<PlayerNettyConnection> localizer) : base(socket)
        {
            ServerInfo = serverInfo;
            NetworkBus = networkBus;
            Localizer = localizer;
        }

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

        protected override void AdditionalWork()
        {
            if (Stream.State == Data.State.Handshake || Stream.State == Data.State.Status)
                return;

            if (PlayerBusId == null)
            {
                Guid? playerBusId = null;

                var awaiter = new ManualResetEvent(true);
                using var _ = NetworkBus.Subscribe<GetExistingPlayerHandlerResponseMessage>(message =>
                {
                    if (message.ServiceId != null)
                    {
                        playerBusId = message.ServiceId;
                        Stream.State = (Data.State) message.State;
                    }
                });
                NetworkBus.Publish(new GetExistingPlayerHandlerRequestMessage()
                {
                    PlayerId = PlayerId,
                    ProtocolVersion = Stream.ProtocolVersion
                });
                awaiter.WaitOne(2000);

                if (playerBusId == null)
                {
                    var response = NetworkBus.PublishAndWaitForExclusiveResponse<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>(
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
                            JSONData = $@"{{ ""text"": ""{Localizer.GetString("login.kicked")}"" }}"
                        });
                    }
                    Disconnect();
                }
                else
                {
                    PlayerBusId = playerBusId;
                    Events.Add(NetworkBus.Subscribe<PlayerDataToProxyMessage>(message =>
                    {
                        try
                        {
                            Stream.Socket.Send(message.Data);
                        }
                        catch (Exception e) when (e is SocketException)
                        {
                            Events.Dispose();
                        }
                    }, PlayerId));
                }
            }
            else
            {
                while (Stream.DataToSend.TryDequeue(out var data))
                    NetworkBus.Publish(new PlayerDataToBusMessage() { Data = data }, PlayerId);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Events?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}