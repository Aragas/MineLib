using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using MineLib.Core.Client;
using MineLib.Core.Loader;
using MineLib.Core.Exceptions;

using ProtocolModern.Packets;
using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.Client
{
    public sealed partial class Protocol : MineLib.Core.Client.Protocol
    {
        static Protocol()
        {
            Extensions.PacketExtensions.Init();
        }

        #region Properties

        public override string Name => "Modern";
        public override string Version => "1.7.10";
        public override int NetworkVersion => 5;

        public override string Host => Stream?.Host ?? string.Empty;
        public override ushort Port => Stream?.Port ?? 0;
        public override bool Connected => Stream?.Connected ?? false;

        private bool IsForge { get; set; }

        #endregion Properties


        #region Debug

#if DEBUG

        private List<ProtobufPacket> PacketsReceived { get; } = new List<ProtobufPacket>();
        private List<ProtobufPacket> PacketsSended { get; } = new List<ProtobufPacket>();
        private List<ProtobufPacket> PluginMessage { get; } = new List<ProtobufPacket>();

        private List<ProtobufPacket> LastPackets => PacketsReceived?.GetRange(PacketsReceived.Count - 50, 50);
        private ProtobufPacket LastPacket => PacketsReceived[PacketsReceived.Count - 1];
#endif

        #endregion Debug


        private IThread ReadThread { get; set; }
        private CancellationTokenSource CancellationToken { get; } = new CancellationTokenSource();

        internal ProtobufStream Stream { get; }


        public Protocol(MineLibClient client, ProtocolPurpose purpose) : base(client, purpose)
        {
            Stream = new ProtobufStream(TCPClientWrapper.Create());

            RegisterSupportedSendings();




            //ModAPIs
            var modules = AssemblyParser.GetAssemblyInfos("Forge*.dll");
            if (modules.Any())
                foreach (var module in modules)
                    LoadForgeModAPI(module);
            else
                LoadForgeModAPI(new AssemblyInfo("NONE"));
        }


        private void ReadCycle()
        {
            while (!CancellationToken.IsCancellationRequested)
                PacketReceiver();
        }
        private void PacketReceiver()
        {
            var packetLength = Stream.ReadVarInt();
            if (packetLength == 0)
                throw new ProtocolException("Reading error: Packet Length size is 0");

            HandleData(Stream.Receive(packetLength));
        }


        /// <summary>
        /// Data is handled here. Compression and encryption are handled here too
        /// </summary>
        /// <param name="data">Packet byte[] data</param>
        private void HandleData(byte[] data)
        {
            using (var reader = new ProtobufDataReader(data))
            {
                var id = reader.Read<VarInt>();
                var packet = default(ProtobufPacket);

                switch (State)
                {
                    #region Status

                    case ClientState.InfoRequest:
                        if (ClientPackets.StatusPacketResponses.Packets[id] == null)
                            throw new ProtocolException("Reading error: Wrong Status packet ID.");

                        packet = ClientPackets.StatusPacketResponses.Packets[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ClientState.InfoRequest);
                        break;

                    #endregion Status

                    #region Login

                    case ClientState.Joining:
                        if (ClientPackets.LoginPacketResponses.Packets[id] == null)
                            throw new ProtocolException("Reading error: Wrong Login packet ID.");

                        packet = ClientPackets.LoginPacketResponses.Packets[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ClientState.Joining);
                        break;

                    #endregion Login

                    #region Play

                    case ClientState.Joined:
                        if (id < 0 || ClientPackets.PlayPacketResponses.Packets[id] == null)
                            break;
                            //throw new ProtocolException("Reading error: Wrong Play packet ID.");

                        packet = ClientPackets.PlayPacketResponses.Packets[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ClientState.Joined);
                        break;

                    #endregion Play
                }

                if(packet == null)
                    return;

#if DEBUG
                if(packet is PluginMessagePacket)
                    PluginMessage.Add(packet);

                PacketsReceived.Add(packet);
#endif
            }
        }
        

        internal void SetState(ClientState state) { State = state; }


        #region Network

        public override IStatusClient CreateStatusClient() => new StatusClient();

        public override void Connect(ServerInfo serverInfo)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            foreach (var modAPI in ModAPIs)
                modAPI.OnConnect(serverInfo);
            

            // -- Connect to server.
            Stream.Connect(serverInfo.Address.IP, serverInfo.Address.Port);

            // -- Begin data reading.
            if (ReadThread != null && ReadThread.IsRunning)
                throw new ProtocolException("Connection error: Thread already running.");
            else
            {
                ReadThread = ThreadWrapper.Create(ReadCycle);
                ReadThread.IsBackground = true;
                ReadThread.Name = "PacketReaderThread";
                ReadThread.Start();
            }

        }
        public override void Disconnect()
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            CancellationToken.Cancel();

            Stream.Disconnect();
        }

        public void SendPacket(ProtobufPacket packet)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            Stream.SendPacket(packet);

#if DEBUG
            PacketsSended.Add(packet);
#endif
        }

        #endregion


        public override void Dispose()
        {
            CancellationToken?.Cancel();

            Stream?.Dispose();

#if DEBUG
            PacketsReceived?.Clear();
            PacketsSended?.Clear();
#endif

            CancellationToken?.Dispose();
        }
    }
}
