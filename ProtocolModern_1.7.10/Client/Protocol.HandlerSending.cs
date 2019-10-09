using System;
using System.Collections.Generic;
using System.Text;

using Aragas.Core.Data;
using Aragas.Core.Packets;

using MineLib.Core.Data.Structs;
using MineLib.Core.Events;
using MineLib.Core.Events.SendingEvents;

using ProtocolModern.Enum;
using ProtocolModern.Packets.Client.Play;
using ProtocolModern.Packets.Server.Handshake;
using ProtocolModern.Packets.Server.Login;
using ProtocolModern.Packets.Server.Play;

namespace ProtocolModern.Client
{
    public sealed partial class Protocol
    {
        private Dictionary<Type, List<Action<SendingEvent>>> SendingHandlers { get; } = new Dictionary<Type, List<Action<SendingEvent>>>();
        private static Action<SendingEvent> TransformSending<TSendingType>(Action<TSendingType> action) where TSendingType : SendingEvent => sendingEvent => action((TSendingType) sendingEvent);

        public override void RegisterSending<TSendingType>(Action<TSendingType> func)
        {
            var sendingType = typeof (TSendingType);

            if (SendingHandlers.ContainsKey(sendingType))
                SendingHandlers[sendingType].Add(TransformSending(func));
            else
                SendingHandlers.Add(sendingType, new List<Action<SendingEvent>> { TransformSending(func) });
        }
        public override void DeregisterSending<TSendingType>(Action<TSendingType> func)
        {
            var sendingType = typeof (TSendingType);

            if (SendingHandlers.ContainsKey(sendingType))
                SendingHandlers[sendingType].Remove(TransformSending(func));
        }
        
        public override void FireEvent<TSendingType>(TSendingType args)
        {
            var sendingType = args.GetType();

            args.RegisterSending(SendPacket);

            List<Action<SendingEvent>> list;
            if (SendingHandlers.TryGetValue(sendingType, out list))
                foreach (var func in list)
                    func(args);
        }
        
        private void RegisterSupportedSendings()
        {
            RegisterSending<ConnectToServer>(ConnectToServer);
            RegisterSending<KeepAliveEvent>(KeepAlive);
            RegisterSending<SendClientInfoEvent>(SendClientInfo);
            RegisterSending<RespawnEvent>(Respawn);
            RegisterSending<PlayerMovedEvent>(PlayerMoved);
            RegisterSending<PlayerSetRemoveBlockEvent>(PlayerSetRemoveBlock);
            RegisterSending<SendMessageEvent>(SendMessage);
            RegisterSending<PlayerHeldItemEvent>(PlayerHeldItem);
        }


        #region InnerSending

        /*
        private async Task ConnectToServerAsync1(SendingArgs args)
        {
            var data = (ConnectToServerArgs) args;

            await args.SendPacketAsync(new HandshakePacket
            {
                ProtocolVersion = 47,
                ServerAddress = Minecraft.ServerHost,
                ServerPort = Minecraft.ServerPort,
                NextState = (int)NextState.Login,
            });

            await args.SendPacketAsync(new LoginStartPacket { Name = Minecraft.ClientUsername });
        }
        */

        private void ConnectToServer(ConnectToServer args) // Forge
        {
            args.SendPacket(new HandshakePacket
            {
                ServerAddress = args.ServerHost + "\0FML\0",
                ServerPort = args.Port,
                ProtocolVersion = new VarInt(NetworkVersion),
                NextState = NextState.Login
            });

            args.SendPacket(new LoginStartPacket { Name = args.Username });


            //await SendPacketAsync(GetFMLFakeLoginPacket());
            //await SendPacketAsync(new ClientStatusPacket { Status = ClientStatus.Respawn});
        }

        private ProtobufPacket GetFMLFakeLoginPacket()
        {
            var input = Encoding.UTF8.GetBytes("FML");
            var murmur3 = new MurmurHash3_32();
            var FML_HASH = BitConverter.ToInt32(murmur3.ComputeHash(input), 0);

            // Always reset compat to zero before sending our fake packet
            JoinGamePacket fake = new JoinGamePacket();
            // Hash FML using a simple function
            fake.EntityID = FML_HASH;
            // The FML protocol version
            fake.Dimension = 2;
            fake.GameMode = 0;
            fake.LevelType = "DunnoLol";
            return fake;
        }

        private void KeepAlive(KeepAliveEvent args)
        {
            args.SendPacket(new KeepAlivePacket { KeepAliveID = new VarInt(args.KeepAlive) });
        }

        private void SendClientInfo(SendClientInfoEvent args)
        {
            args.SendPacket(new PluginMessagePacket
            {
                Channel = "MC|Brand",
                Data = Encoding.UTF8.GetBytes(Client.Name)
            });
        }

        private void Respawn(RespawnEvent args)
        {
            args.SendPacket(new ClientStatusPacket { ActionID = (sbyte) ClientStatus.Respawn });
        }

        private void PlayerMoved(PlayerMovedEvent args)
        {
            switch (args.Mode)
            {
                case PlaverMovedMode.OnGround:
                {
                    var pdata = (PlaverMovedDataOnGround) args.Data;

                    args.SendPacket(new PlayerPacket
                    {
                        OnGround = pdata.OnGround
                    });
                    break;
                }

                case PlaverMovedMode.Vector3:
                {
                    var pdata = (PlaverMovedDataVector3) args.Data;

                    args.SendPacket(new PlayerPositionPacket
                    {
                        X = pdata.Vector3.X,
                        FeetY = pdata.Vector3.Y,
                        Z = pdata.Vector3.Z,
                        OnGround = pdata.OnGround
                    });
                    break;
                }

                case PlaverMovedMode.YawPitch:
                {
                    var pdata = (PlaverMovedDataYawPitch) args.Data;

                    args.SendPacket(new PlayerLookPacket
                    {
                        Yaw =       pdata.Yaw,
                        Pitch =     pdata.Pitch,
                        OnGround =  pdata.OnGround
                    });
                    break;
                }

                case PlaverMovedMode.All:
                {
                    var pdata = (PlaverMovedDataAll) args.Data;

                    args.SendPacket(new PlayerPositionAndLook2Packet
                    {
                        X =         pdata.Vector3.X,
                        FeetY =     pdata.Vector3.Y,
                        Z =         pdata.Vector3.Z,
                        Yaw =       pdata.Yaw,
                        Pitch =     pdata.Pitch,
                        OnGround =  pdata.OnGround
                    });
                    break;
                }

                default:
                    return;
            }
        }

        private void PlayerSetRemoveBlock(PlayerSetRemoveBlockEvent args)
        {
            switch (args.Mode)
            {
                case PlayerSetRemoveBlockMode.Place:
                {
                    var pdata = (PlayerSetRemoveBlockDataPlace) args.Data;

                    args.SendPacket(new PlayerBlockPlacementPacket
                    {
                        X =                     pdata.Location.X,
                        Y =                     (byte) pdata.Location.Y,
                        Z =                     pdata.Location.Z,
                        HeldItem =              pdata.Slot,
                        CursorPositionX =       (sbyte) pdata.Crosshair.X,
                        CursorPositionY =       (sbyte) pdata.Crosshair.Y,
                        CursorPositionZ =       (sbyte) pdata.Crosshair.Z,
                        Direction =             (sbyte) pdata.Direction
                    });
                    break;
                }

                case PlayerSetRemoveBlockMode.Dig:
                {
                    var pdata = (PlayerSetRemoveBlockDataDig) args.Data;

                    args.SendPacket(new PlayerDiggingPacket
                    {
                        Status =                (sbyte) pdata.Status,
                        X =                     pdata.Location.X,
                        Y =                     (byte) pdata.Location.Y,
                        Z =                     pdata.Location.Z,
                        Face =                  pdata.Face
                    });
                    break;
                }

                case PlayerSetRemoveBlockMode.Remove:
                {
                    var pdata = (PlayerSetRemoveBlockDataRemove) args.Data;
                    break;
                }

                default:
                    throw new Exception("PacketError");
            }
        }

        private void SendMessage(SendMessageEvent args)
        {
            args.SendPacket(new ChatMessage2Packet { Message = args.Message });
        }

        private void PlayerHeldItem(PlayerHeldItemEvent args)
        {
            args.SendPacket(new HeldItemChange2Packet { Slot = args.Slot });
        }

        #endregion InnerSending
    }
}
