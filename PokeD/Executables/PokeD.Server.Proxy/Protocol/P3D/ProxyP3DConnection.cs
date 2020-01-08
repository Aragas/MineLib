using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus.Messages;

using PokeD.Core.IO;
using PokeD.Core.Packets.P3D;
using PokeD.Core.Packets.P3D.Client;
using PokeD.Core.Packets.P3D.Server;
using PokeD.Server.Core.NetworkBus.Messages;

using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PokeD.Server.Proxy.Protocol.P3D
{
    internal sealed class ProxyP3DConnection : DefaultConnectionHandler<ProxyP3DTransmission, P3DPacket, int, P3DSerializer, P3DDeserializer>
    {
        private Guid? PlayerBusId { get; set; }

        private Guid PlayerId { get; } = Guid.NewGuid();

        private IDisposable PlayerBusDataEvent { get; set; }  

        private ConcurrentQueue<byte[]> DataToSend { get; } = new ConcurrentQueue<byte[]>();

        /// <summary>
        /// For internal use only.
        /// </summary>
        public ProxyP3DConnection() { }
        public ProxyP3DConnection(Socket socket) : base(socket) { }

        protected override void HandlePacket(P3DPacket p3dPacket)
        {
            switch (p3dPacket)
            {
                case PingPacket pingPacket:
                    break;

                case ServerDataRequestPacket serverDataRequestPacket:
                    HandleServerDataRequest(serverDataRequestPacket);
                    Task.Run(Disconnect);
                    break;
            }
        }

        private void HandleServerDataRequest(ServerDataRequestPacket packet)
        {
            //var clientNames = Module.AllClientsSelect(clients => clients.Select(client => client.Name).ToList());
            SendPacket(new ServerInfoDataPacket
            {
                //Origin = ID,
                Origin = -1,

                CurrentPlayers = Program.CurrentConnections,
                MaxPlayers = Program.MaxConnections,
                PlayerNames = new string[0],

                ServerName = Program.Name,
                ServerMessage = Program.Description,
            });
        }

        private bool HasInit = false;
        protected override void AdditionalWork()
        {
            if (Stream != null && !HasInit)
            {
                HasInit = true;

                Stream.OnDataReceived += data => DataToSend.Enqueue(data);
            }

            if (Stream.State == 0)
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
                        //State = (Data.State)message.State;
                    }
                });
                BaseSingleton.Instance.Publish(new GetExistingPlayerHandlerRequestMessage()
                {
                    PlayerId = PlayerId,
                    //ProtocolVersion = ProtocolVersion
                });
                awaiter.WaitOne(2000);

                if (playerBusId == null)
                {
                    var response = BaseSingleton.Instance.PublishAndWaitForExclusiveResponse<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>(
                        new GetNewPlayerHandlerRequestMessage()
                        {
                            PlayerId = PlayerId,
                            //ProtocolVersion = ProtocolVersion
                        });
                    if (response != null)
                        playerBusId = response.ServiceId;
                }


                if (playerBusId == null)
                {
                    /*
                    if (State == Data.State.Login)
                    {
                        SendPacket(new Disconnect2Packet()
                        {
                            JSONData = $@"{{ ""text"": ""{Strings.LoginKicked}"" }}"
                        });
                    }
                    */
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
                PlayerBusDataEvent.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}