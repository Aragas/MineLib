using System;
using System.Collections.Generic;

namespace Aragas.Network.IO
{
    public interface IPacketSerializer : IDisposable
    {
        void Write<TDataType>(TDataType value = default, bool writeDefaultLength = true);
    }
    public abstract class PacketSerializer : IPacketSerializer
    {
        #region ExtendWrite

        private static readonly Dictionary<int, Action<StreamSerializer, object?, bool>> WriteExtendedList = new Dictionary<int, Action<StreamSerializer, object?, bool>>();

        public static void ExtendWrite<T>(Action<StreamSerializer, T, bool> action)
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            if (action != null)
#pragma warning disable CS8601 // Possible null reference assignment.
                WriteExtendedList.Add(type.GetHashCode(), (stream, value, writeDefaultLength) => action(stream, (T) value, writeDefaultLength));
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        protected static bool ExtendWriteContains<T>()
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            return ExtendWriteContains(type);
        }
        protected static bool ExtendWriteContains(Type type) => WriteExtendedList.ContainsKey(type.GetHashCode());

        protected static void ExtendWriteExecute<T>(StreamSerializer stream, T value, bool writeDefaultLength = true)
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            if (WriteExtendedList.TryGetValue(type.GetHashCode(), out var action))
                action.Invoke(stream, value, writeDefaultLength);
        }

        #endregion ExtendWrite

        private bool disposed = false;

        public abstract void Write<TDataType>(TDataType value = default, bool writeDefaultLength = true);

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

        ~PacketSerializer()
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