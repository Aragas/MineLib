using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

using Aragas.Network.IO;
using Aragas.Network.Packets;

using PokeD.Core.Packets.P3D;

namespace PokeD.Core.IO
{
    public class P3DTransmission : SocketPacketTransmission<P3DPacket, int, P3DSerializer, P3DDeserializer>
    {
        [DllImport("libc", EntryPoint = "setsockopt")]
        private static extern int SetSocketOptionLibC(IntPtr socket, int level, int optname, ref int optval, uint optlen);

        private static bool SetKeepAlive(Socket socket, ulong time, ulong interval)
        {
            const int bytesPerLong = 4;
            const int bitsPerByte = 8;

            var turnOn = time != 0 && interval != 0;
            // Array to hold input values.
            var input = new[]
            {
                turnOn ? 1UL : 0UL, // on or off
                time,
                interval
            };

            // Pack input into byte struct.
            var inValue = new byte[3 * bytesPerLong];
            for (var i = 0; i < input.Length; i++)
            {
                inValue[(i * bytesPerLong) + 3] = (byte)(input[i] >> ((bytesPerLong - 1) * bitsPerByte) & 0xff);
                inValue[(i * bytesPerLong) + 2] = (byte)(input[i] >> ((bytesPerLong - 2) * bitsPerByte) & 0xff);
                inValue[(i * bytesPerLong) + 1] = (byte)(input[i] >> ((bytesPerLong - 3) * bitsPerByte) & 0xff);
                inValue[(i * bytesPerLong) + 0] = (byte)(input[i] >> ((bytesPerLong - 4) * bitsPerByte) & 0xff);
            }

            var outValue = BitConverter.GetBytes(0);

            try
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, turnOn);
                socket.IOControl(IOControlCode.KeepAliveValues, inValue, outValue);
            }
            catch (Exception e) when (e is SocketException) { return false; }
            return true;
        }

        public override Socket Socket
        {
            get => _socket!; // Because we can't create a Generic with new() with properties
            set
            {
                if (_socket != null)
                    throw new InvalidOperationException("You can't set Socket once it's initialized");
                _socket = value;
                Stream = new P3DSocketStream(Socket);
            }
        }

        public P3DTransmission() : base() { }
        public P3DTransmission(Socket socket) : this(socket, null) { }
        public P3DTransmission(Socket socket, BasePacketFactory<P3DPacket, int, P3DSerializer, P3DDeserializer> factory) : base(socket, new P3DSocketStream(socket), factory)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SetKeepAlive(socket, 5000, 1000);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Even if this is not intended for checking if the connection is alive, this is the best way for now
                var onOff = 1;
                var keepAliveTimeSeconds = 5;
                var keepAliveIntervalSeconds = 1;
                var keepAliveCount = 3;

                SetSocketOptionLibC(socket.Handle, 0xFFFF, 0x8,    ref onOff, sizeof(int));
                SetSocketOptionLibC(socket.Handle, 0x6,    0x4,    ref keepAliveTimeSeconds, sizeof(int));
                SetSocketOptionLibC(socket.Handle, 0x6,    0x5,    ref keepAliveIntervalSeconds, sizeof(int));
                SetSocketOptionLibC(socket.Handle, 0x6,    0x6,    ref keepAliveCount, sizeof(int));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // Even if this is not intended for checking if the connection is alive, this is the best way for now
                var onOff = 1;
                var keepAliveTimeSeconds = 5;
                var keepAliveIntervalSeconds = 1;
                var keepAliveCount = 3;

                SetSocketOptionLibC(socket.Handle, 0xFFFF, 0x8,    ref onOff, sizeof(int));
                SetSocketOptionLibC(socket.Handle, 0x6,    0x10,   ref keepAliveTimeSeconds, sizeof(int));
                SetSocketOptionLibC(socket.Handle, 0x6,    0x101,  ref keepAliveIntervalSeconds, sizeof(int));
                SetSocketOptionLibC(socket.Handle, 0x6,    0x102,  ref keepAliveCount, sizeof(int));
            }
        }


        public override void SendPacket(P3DPacket packet)
        {
            Send(Encoding.UTF8.GetBytes($"{packet.CreateData()}\r\n"));
        }

        public override P3DPacket? ReadPacket()
        {
            var data = ReadLine();

            if (P3DPacket.TryParseID(data, out var id))
            {
                var packet = Factory.Create(id);
                if (packet?.TryParseData(data) == true)
                    return packet;
            }

            return null;
        }

        private StringBuilder StringBuilder { get; } = new StringBuilder();
        private IEnumerable<string> ReadLineEnumerable()
        {
            var @byte = Stream.ReadByte();
            var symbol = (char) @byte;
            while (@byte != -1)
            {
                var nextSymbol = char.MinValue;
                if (symbol == '\r' && Socket.Available == 0)
                {
                    var line = StringBuilder.ToString();
                    StringBuilder.Clear();

                    yield return line;
                }
                else if ((@byte = Stream.ReadByte()) != -1 && (nextSymbol = (char) @byte) == '\n' && symbol == '\r')
                {
                    var line = StringBuilder.ToString();
                    StringBuilder.Clear();

                    yield return line;
                }
                else if (@byte == -1)
                    yield return string.Empty;
                else
                {
                    StringBuilder.Append(symbol);
                    symbol = nextSymbol;
                }
            }
            yield return string.Empty;
        }
        public string ReadLine() => ReadLineEnumerable().First();
    }
}