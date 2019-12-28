using Aragas.Network.Data;
using Aragas.Network.IO;

using PokeD.Server.Core;
using PokeD.Server.Proxy.Packets.P3D;
using PokeD.Server.Proxy.Packets.P3D.Clientbound;
using PokeD.Server.Proxy.Protocol.Data;
using PokeD.Server.Proxy.Protocol.Factory.P3D;

using System.Collections.Concurrent;
using System.Net.Sockets;

namespace PokeD.Server.Proxy.Protocol.P3D
{
    /// <summary>
    /// This class is not related and should not be related to MineLib.Protocol.* classes
    /// </summary>
    internal sealed class ProxyP3DTransmission : ProtobufTransmission<ProxyP3DPacket>
    {
        private Socket PlayerHandlerSocket { get; set; }
        private VarInt ProtocolVersion { get; set; }
        private ConcurrentQueue<byte[]> DataToSend { get; } = new ConcurrentQueue<byte[]>();


        public State State { get; set; } = State.Handshake;

        private HandshakeStateFactory HandshakeFactory { get; } = new HandshakeStateFactory();
        private StatusStateFactory StatusFactory { get; } = new StatusStateFactory();

        /// <summary>
        /// For internal use only.
        /// </summary>
        public ProxyP3DTransmission() : base() { }
        //public ProxyP3DTransmission(Socket socket) : base(socket, null, new EmptyFactory()) { }

        public override ProxyP3DPacket ReadPacket()
        {
            if (Socket.Available > 0)
            {
                if (State == State.Login || State == State.Play)
                {
                    var buffer = new byte[Socket.Available];
                    Socket.Receive(buffer);
                    DataToSend.Enqueue(buffer);
                    return null;
                }

                using var deserializer = new ProtobufDeserializer(Stream);
                var id = deserializer.Read<VarInt>();
                ProxyP3DPacket packet;
                switch (State)
                {
                    case State.Handshake:
                        packet = HandshakeFactory.Create(id);
                        break;
                    case State.Status:
                        packet = StatusFactory.Create(id);
                        break;

                    default:
                        packet = null;
                        break;
                }

                if (packet != null)
                {
                    packet.Deserialize(deserializer);

                    /*
                    if (packet is HandshakePacket handshakePacket)
                    {
                        ProtocolVersion = handshakePacket.ProtocolVersion;
                        State = (State) (byte) handshakePacket.NextState;
                    }
                    */

                    return packet;
                }
            }

            return null;
        }

        public bool TryReadPacket(out ProxyP3DPacket packet) => (packet = ReadPacket()) != null;

        public void DoProxyIO()
        {
            if(State == State.Handshake || State == State.Status)
                return;

            if (PlayerHandlerSocket == null)
            {
                // TODO: Move it to a more appropriate place
                var socket = InternalBus.GetFirstAvailablePlayerHandlerConnection(ProtocolVersion);
                if (socket == null)
                {
                    if (State == State.Login)
                        SendPacket(new Disconnect2Packet()
                        {
                            //JSONData = $@"{{ ""text"": ""{Strings.LoginKicked}"" }}"
                        });
                    Disconnect();
                }
                else
                    PlayerHandlerSocket = socket;
            }
            else
            {
                try // Even if we check Connected, it still can make an exception
                {
                    while (PlayerHandlerSocket?.Available > 0)
                    {
                        var buffer = new byte[PlayerHandlerSocket.Available];
                        PlayerHandlerSocket.Receive(buffer);
                        Socket.Send(buffer);
                    }
                    while (DataToSend.TryDequeue(out var data))
                        PlayerHandlerSocket.Send(data);
                }
                catch (SocketException) {
                    //Disconnect();
                }
            }
        }

        public override void Disconnect()
        {
            if (PlayerHandlerSocket?.Connected == false)
                PlayerHandlerSocket.Disconnect(false);

            base.Disconnect();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DataToSend.Clear();
                PlayerHandlerSocket?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}