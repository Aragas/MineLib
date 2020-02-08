using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

using Aragas.Network.Packets;

using PokeD.Core.Data.P3D;

namespace PokeD.Core.Packets.P3D
{
    public readonly struct Origin
    {
        public static Origin Server => new Origin(-1);

        public static implicit operator Origin(int origin) => new Origin(origin);
        public static implicit operator int(Origin origin) => origin._value;

        private readonly int _value;
        private Origin(int origin) => _value = origin;

        public override string ToString() => _value.ToString();
        public string ToString(IFormatProvider provider) => _value.ToString(provider);


        public static bool operator ==(Origin left, Origin right) => left._value == right._value;
        public static bool operator !=(Origin left, Origin right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Origin origin && Equals(origin);
        public bool Equals(Origin other) => other._value.Equals(_value);

        public override int GetHashCode() => HashCode.Combine(_value);
    }

    public abstract class P3DPacket : PacketWithIDAttribute<int>
    {
        public Origin Origin { get; set; }

        private static float _protocolVersion = 0.5f;
        public static float ProtocolVersion
        {
            get { return _protocolVersion; }
            set
            {
                _protocolVersion = value;
                ProtocolVersionString = ProtocolVersion.ToString(CultureInfo);
            }
        }
        public static string ProtocolVersionString { get; private set; } = ProtocolVersion.ToString(CultureInfo);

        public DataItems DataItems = new DataItems();

        protected static CultureInfo CultureInfo => CultureInfo.InvariantCulture;

        public static bool TryParseID(ReadOnlySpan<char> span, out int id)
        {
            var scanned = -1;
            var position = 0;


            id = 0;

            if (!span.Contains("|", StringComparison.Ordinal))
                return false;

            ParseChunk(ref span, ref scanned, ref position); // skip first
            return int.TryParse(ParseChunk(ref span, ref scanned, ref position), out id);
        }

        public bool TryParseData(ReadOnlySpan<char> span)
        {
            var scanned = -1;
            var position = 0;


            if (!ParseChunk(ref span, ref scanned, ref position).Equals(ProtocolVersionString, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!int.TryParse(ParseChunk(ref span, ref scanned, ref position), out _))
                return false;


            if (!int.TryParse(ParseChunk(ref span, ref scanned, ref position), out var origin))
                return false;
            else
                Origin = origin;

            if (!int.TryParse(ParseChunk(ref span, ref scanned, ref position), out var dataItemsCount))
                return false;

            Span<int> offsets = dataItemsCount * 4 < 1024 ? stackalloc int[dataItemsCount] : new int[dataItemsCount];

            //Count from 4th item to second last item. Those are the offsets.
            for (var i = 0; i < dataItemsCount; i++)
            {
                if (!int.TryParse(ParseChunk(ref span, ref scanned, ref position), out var offset))
                    return false;
                else
                    offsets[i] = offset;
            }

            //Set the datastring, its the last item in the list. If it contained any separators, they will get read here:
            scanned += position + 1;
            var dataString = span.Slice(scanned, span.Length - scanned);

            //Cutting the data:
            for (var i = 0; i < offsets.Length; i++)
            {
                var cOffset = offsets[i];
                var length = dataString.Length - cOffset;

                if (i < offsets.Length - 1)
                    length = offsets[i + 1] - cOffset;

                if (length < 0)
                    return false;

                if (cOffset + length > dataString.Length)
                    return false;

                DataItems.AddToEnd(dataString.Slice(cOffset, length));
            }

            return true;
        }
        private static ReadOnlySpan<char> ParseChunk(ref ReadOnlySpan<char> span, ref int scanned, ref int position)
        {
            scanned += position + 1;

            position = span.Slice(scanned, span.Length - scanned).IndexOf('|');
            if (position < 0)
            {
                position = span.Slice(scanned, span.Length - scanned).Length;
            }

            return span.Slice(scanned, position);
        }


        public string CreateData()
        {
            var dataItems = DataItems.ToArray();

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(ProtocolVersion.ToString(CultureInfo));
            stringBuilder.Append("|");
            stringBuilder.Append(ID.ToString(CultureInfo));
            stringBuilder.Append("|");
            stringBuilder.Append(Origin.ToString(CultureInfo));

            if (dataItems.Length == 0)
            {
                stringBuilder.Append("|0|");
                return stringBuilder.ToString();
            }

            stringBuilder.Append("|");
            stringBuilder.Append(dataItems.Length.ToString(CultureInfo));
            stringBuilder.Append("|0|");

            var num = 0;
            for (var i = 0; i < dataItems.Length - 1; i++)
            {
                num += dataItems[i].Length;
                stringBuilder.Append(num);
                stringBuilder.Append("|");
            }

            foreach (var dataItem in dataItems)
                stringBuilder.Append(dataItem);

            return stringBuilder.ToString();
        }
    }
}
