using System;
using System.Collections.Generic;

namespace Aragas.Network.IO
{
    public abstract class PacketDeserializer : IDisposable
    {
        #region ExtendRead

        private static readonly Dictionary<int, Func<StreamDeserializer, int, object>> ReadExtendedList = new Dictionary<int, Func<StreamDeserializer, int, object>>();

        public static void ExtendRead<T>(Func<StreamDeserializer, int, T> func)
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            if (func != null)
                ReadExtendedList.Add(type.GetHashCode(), Transform(func));
        }

        private static Func<StreamDeserializer, int, object> Transform<T>(Func<StreamDeserializer, int, T> action) =>
            (reader, length) =>
                action(reader, length);

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

            return ExtendReadContains<T>() ? (T) ReadExtendedList[type.GetHashCode()](reader, length) : default;
        }

        protected static bool ExtendReadTryExecute<T>(StreamDeserializer reader, int length, out T value)
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            var exist = ReadExtendedList.TryGetValue(type.GetHashCode(), out var func);
            value = exist ? (T) func.Invoke(reader, length) : default;

            return exist;
        }

        #endregion ExtendRead

        protected PacketDeserializer() { }
        protected PacketDeserializer(in Span<byte> data) => Initialize(in data);

        protected abstract void Initialize(in Span<byte> data);

        public abstract T Read<T>(T value = default, int length = 0);

        public abstract int BytesLeft();

        public abstract void Dispose();
    }
}