using System;
using System.Runtime.CompilerServices;

namespace Aragas.QServer.NetworkBus.Messages
{
    public abstract class BinaryMessage : IMessage
    {
        public abstract string Name { get; }
        public byte[] Data { get; set; } = default!;

        public ReadOnlySpan<byte> GetData() => Data;
        public void SetData(in ReadOnlySpan<byte> data) => Data = data.ToArray();
    }
    public abstract class BinaryEnumerableMessage : IEnumerableMessage
    {
        private bool _isLastMessage = default!;
        public bool IsLastMessage => _isLastMessage;

        public abstract string Name { get; }
        public byte[] Data { get; set; } = default!;

        protected BinaryEnumerableMessage(bool isLast) { _isLastMessage = isLast; }

        public ReadOnlySpan<byte> GetData()
        {
            Span<byte> data = new byte[Data.Length + 1];
            data[0] = Unsafe.As<bool, byte>(ref _isLastMessage);
            Data.CopyTo(data.Slice(1));
            return data;
        }
        public void SetData(in ReadOnlySpan<byte> data)
        {
            _isLastMessage = Unsafe.As<byte, bool>(ref Unsafe.AsRef(in data[0]));
            Data = data.Slice(1).ToArray();
        }
    }
}