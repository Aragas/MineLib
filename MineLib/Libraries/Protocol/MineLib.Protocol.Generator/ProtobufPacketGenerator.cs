using Aragas.Network.Data;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Protocol5.Data;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MineLib.Protocol.Generator
{
    internal class Field
    {
        private static readonly Regex AnyStringLength = new Regex(@"String\(\d+\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex AnyStringEnumLength = new Regex(@"StringEnum\(\d+\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex Optional = new Regex(@"Optional(.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static List<string> UnrecognizedTypes = new List<string>();
        public static string? ReplaceTypes(string name, string type)
        {
            switch (type)
            {
                case "UUID":
                    return typeof(Guid).Name;

                case "Varlong":
                    return typeof(VarLong).Name;
                case "VarInt":
                case "Varint":
                case "VarintEnum":
                    return typeof(VarInt).Name;

                case "String":
                    return typeof(string).Name;

                case "UnsignedShort":
                    return typeof(ushort).Name;

                case "UnsignedByte":
                    return typeof(byte).Name;

                case "Int":
                case "Integer":
                case "IntEnum":
                    return typeof(int).Name;

                case "Byte":
                case "ByteEnum":
                    return typeof(sbyte).Name;

                case "Boolean":
                case "Bool":
                    return typeof(bool).Name;

                case "Chat":
                    return typeof(string).Name;

                case "Long":
                    return typeof(long).Name;

                case "Short":
                    return typeof(short).Name;

                case "Slot":
                    return typeof(ItemSlot).Name;

                case "Position":
                    return typeof(Location3D).Name;

                case "Float":
                    return typeof(float).Name;

                case "Double":
                    return typeof(double).Name;

                case "Angle":
                    return typeof(byte).Name;

                case "Metadata":
                case "EntityMetadata":
                    return typeof(EntityMetadataList).Name;

                case "Meta":
                    return typeof(ChunkColumnMetadata[]).Name;

                case "ObjectData":
                    return "NOT_SUPPORTED";

                case "ArrayOfPropertyData":
                    return typeof(EntityProperty[]).Name;

                case "ArrayOfVarInt":
                case "ArrayOfVarint":
                    return typeof(VarInt[]).Name;

                case "Chunk":
                    return typeof(Chunk).Name;

                case "Arrayof(Byte,Byte,Byte)":
                case "ArrayOf(Byte,Byte,Byte)":
                case "ArrayOfUnsignedByte":
                    return typeof(byte[]).Name;

                case "ArrayOfInt":
                    return typeof(int[]).Name;

                case "ArrayOfSlots":
                case "ArrayOfSlot":
                    return typeof(ItemSlot[]).Name;

                case "ArrayOfRecords":
                    return typeof(BlockLocation[]).Name;

                case "ArrayOfString":
                case "ArrayOfStrings":
                    return typeof(string[]).Name;

                case "ByteArray":
                case "(Byte,Byte,Byte)×Count":
                    return typeof(byte[]).Name;

                case "ArrayOf2048Bytes":
                    return typeof(NibbleArray).Name;

                case "Array":
                    return $"{name}[]";

                case "Identifier":
                    return typeof(string).Name;

                case "ArrayOfIdentifier":
                    return typeof(string[]).Name;

                case "ArrayOfIdentifier,OnlyPresentIfModeIs0(Init)":
                case "(SeeBelow)":
                    return "NOT_SUPPORTED";

                default:
                    if (AnyStringLength.IsMatch(type) || AnyStringEnumLength.IsMatch(type))
                        return typeof(string).Name;

                    if (Optional.Match(type) is Match match && match.Success)
                        return $"Nullable<{ReplaceTypes(name, match.Groups[1].Value)}>";

                    if (!string.IsNullOrEmpty(type))
                        UnrecognizedTypes.Add(type);
                    return type;
            }
        }

        public string OriginalName { get; }
        public string Name { get; }
        public string? Type { get; }

        public List<Field> SubFields = new List<Field>();

        public Field(string name, string type)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            OriginalName = name;
            Name = textInfo.ToTitleCase(name)
                .Replace(" ", "")
                .Replace("?", "")
                .Replace("/", "_")
                .Replace("(Byte1)", "").Replace("(Byte2)", "")
                .Replace("NoFields", "");
            Type = ReplaceTypes(Name, textInfo.ToTitleCase(type)
                .Replace(" ", ""))
                .Replace("SkyLightArrays[]", "NibbleArray")
                .Replace("BlockLightArrays[]", "NibbleArray")
                .Replace("NoFields", "")
                .Replace("Nullable<Byte[]>", "Byte[]?")
                .Replace("Nullable<String>", "String?");
        }

        public override string ToString() => $"{Name}_{Type}_[{SubFields.Count}]";
    }
    internal class Packet
    {
        public string Name;
        public string PacketID;
        public string State;
        public string BoundTo;

        public List<Field> Fields = new List<Field>();

        public Packet(string name, string packetID, string state, string boundTo)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            Name = textInfo.ToTitleCase(name)
                .Replace(" ", "")
                .Replace("(", "_").Replace(")", "_")
                .Replace("-", "_") + "Packet";
            PacketID = packetID;
            State = state;
            BoundTo = boundTo;
        }


        public override string ToString() => $"{PacketID}_{Name}_{State}_{BoundTo}_[{Fields.Count}]";
    }

    public enum BoundTo { NONE, Client, Server, Share }
    public enum State { NONE, Handshaking, Play, Status, Login }

    internal class ProtobufPacketGenerator
    {
        private string GenerateFields(Packet packet)
        {
            var builder = new StringBuilder();
            var fields = packet.Fields.Where(f => !string.IsNullOrEmpty(f.Name) && !string.IsNullOrEmpty(f.Type)).ToList();
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i].Name;
                builder.Append("\t\t").Append("public ").Append(fields[i].Type).Append(' ').Append(field).Append(';');
                if (i != fields.Count - 1)
                    builder.AppendLine();
            }

            return builder.ToString();
        }
        private string GenerateReadPacket(Packet packet)
        {
            var builder = new StringBuilder();
            var fields = packet.Fields.Where(f => !string.IsNullOrEmpty(f.Name) && !string.IsNullOrEmpty(f.Type)).ToList();
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i].Name;
                builder.Append("\t\t\t").Append(fields[i].Name).Append(" = deserializer.Read(").Append(field).Append(");");
                if (i != fields.Count - 1)
                    builder.AppendLine();
            }

            return builder.ToString();
        }
        private string GenerateWritePacket(Packet packet)
        {
            var builder = new StringBuilder();
            var fields = packet.Fields.Where(f => !string.IsNullOrEmpty(f.Name) && !string.IsNullOrEmpty(f.Type)).ToList();
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i].Name;
                builder.Append("\t\t\t").Append("serializer.Write(").Append(field).Append(");");
                if (i != fields.Count - 1)
                    builder.AppendLine();
            }

            return builder.ToString();
        }

        public IEnumerable<(Packet, string)> GenerateClasses(IEnumerable<Packet> packets)
        {
            foreach (var packet in packets)
            {
                yield return (packet, $@"
using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.Extensions;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Protocol.Packets;

using System;

namespace MineLib.PacketBuilder
{{
    [Packet({packet.PacketID})]
    public class {packet.Name} : MinecraftPacket
    {{
{GenerateFields(packet)}

        public override void Deserialize(IPacketDeserializer deserializer)
        {{
{GenerateReadPacket(packet)}
        }}

        public override void Serialize(IPacketSerializer serializer)
        {{
{GenerateWritePacket(packet)}          
        }}
    }}
}}");
            }
        }
    }
}