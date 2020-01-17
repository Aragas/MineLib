using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

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
        ""max"": {MineLibOptions.MaxConnections},
        ""online"": {ServerInfo.CurrentConnections}
    }},
    ""name"":
    {{
        ""text"": ""{MineLibOptions.Name}""
    }},
    ""description"":
    {{
        ""text"": ""{MineLibOptions.Description}""
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
        private MineLibOptions MineLibOptions { get; }
        private ServerInfo ServerInfo { get; }
        private IStringLocalizer Localizer { get; }

        public PlayerNettyConnection(IServiceProvider serviceProvider, Socket socket) : base(serviceProvider, socket)
        {
            MineLibOptions = serviceProvider.GetRequiredService<IOptions<MineLibOptions>>().Value;
            ServerInfo = serviceProvider.GetRequiredService<ServerInfo>();
            NetworkBus = serviceProvider.GetRequiredService<INetworkBus>();
            Localizer = serviceProvider.GetRequiredService<IStringLocalizer<PlayerNettyConnection>>();
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

                case ServerListPingPacket serverListPingPacket:
                    var serializer = new StandardSerializer();
                    serializer.Write<byte>(0xFF);
                    // if serverListPingPacket.Payload == 0x00 : ??-1.3.2 (??-39)
                    // if serverListPingPacket.Payload == 0x01 : 1.4-1.5.2 (47-61)
                    var protocolVersion = serverListPingPacket.ProtocolVersion == 0
                        ? MineLibOptions.NettyLegacyPingProtocol // Seems that we cannot support every client in 1.4-1.5.2 range. Only a specific one.
                        : serverListPingPacket.ProtocolVersion;
                    var response = serverListPingPacket.Payload == 0x01
                        ? $"§1\0{protocolVersion}\0Any Version\0{MineLibOptions.Description}\0{ServerInfo.CurrentConnections}\0{MineLibOptions.MaxConnections}"
                        : $"{MineLibOptions.Description}§{ServerInfo.CurrentConnections}§{MineLibOptions.MaxConnections}";
                    serializer.Write<UTF16BEString>(response);
                    Stream.Socket.Send(serializer.GetData().ToArray(), SocketFlags.None);
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