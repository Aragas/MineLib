using Aragas.Network.Packets;

using PokeD.Core.IO;
using PokeD.Core.Packets.P3D;
using PokeD.Core.Packets.P3D.Shared;

using System;
using System.Text;

namespace PokeD.Server.Proxy.Protocol.P3D
{
    internal sealed class ProxyP3DTransmission : P3DTransmission
    {
        public event Action<byte[]> OnDataReceived;

        public int State { get; set; } = 0;

        private BasePacketFactory<P3DPacket, int> P3DFactory { get; } = new DefaultPacketFactory<P3DPacket, int>();

        public override P3DPacket? ReadPacket()
        {
            var data = ReadLine();

            if (State == 0)
            {
                if (P3DPacket.TryParseID(data, out var id))
                {
                    var packet = P3DFactory.Create(id);
                    if (packet?.TryParseData(data) == true)
                    {
                        if (packet is GameDataPacket)
                        {
                            State = 1;
                            OnDataReceived?.Invoke(Encoding.UTF8.GetBytes(data));
                        }

                        return packet;
                    }
                }

                return null;
            }
            else
            {
                OnDataReceived?.Invoke(Encoding.UTF8.GetBytes(data));
                return null;
            }
        }
    }
}