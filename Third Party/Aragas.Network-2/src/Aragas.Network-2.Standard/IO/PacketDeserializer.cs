using System;
using System.Collections.Generic;

namespace Aragas.Network.IO
{
    public interface IPacketDeserializer : IDisposable
    {
        int BytesLeft { get; }

        T Read<T>(T value = default, int length = 0);
    }
    public abstract class PacketDeserializer : IPacketDeserializer
    {
        #region ExtendRead

        private static readonly Dictionary<int, Func<StreamDeserializer, int, object?>> ReadExtendedList = new Dictionary<int, Func<StreamDeserializer, int, object?>>();

        public static void ExtendRead<T>(Func<StreamDeserializer, int, T> func)
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            ReadExtendedList.Add(type.GetHashCode(), (reader, length) => func(reader, length));
        }

        protected static bool ExtendReadContains<T>()
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            return ExtendReadContains(type);
        }

        protected static bool ExtendReadContains(Type type) => ReadExtendedList.ContainsKey(type.GetHashCode());

        protected static T ExtendReadExecute<T>(StreamDeserializer reader, int length = 0)
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

#pragma warning disable CS8601 // Possible null reference assignment.
            return ExtendReadContains<T>() ? (T) ReadExtendedList[type.GetHashCode()](reader, length) : default;
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        protected static bool ExtendReadTryExecute<T>(StreamDeserializer reader, int length, out T value)
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            var exist = ReadExtendedList.TryGetValue(type.GetHashCode(), out var func);
#pragma warning disable CS8601 // Possible null reference assignment.
            value = exist ? (T) func.Invoke(reader, length) : default;
#pragma warning restore CS8601 // Possible null reference assignment.

            return exist;
        }

        #endregion ExtendRead

        private bool disposed = false;

        public abstract int BytesLeft { get; }

        protected PacketDeserializer() { }
        protected PacketDeserializer(in Span<byte> data) => Initialize(in data);

        protected abstract void Initialize(in Span<byte> data);

        public abstract T Read<T>(T value = default, int length = 0);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }


                disposed = true;
            }
        }

         ~PacketDeserializer()
         {
            Dispose(false);
         }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}