using System;

using Aragas.Network.Data;
using Aragas.Network.IO;

using PokeD.BattleEngine.Battle;
using PokeD.BattleEngine.Monster.Data;
using PokeD.BattleEngine.Trainer.Data;
using PokeD.Core.Data;
using PokeD.Core.Data.PokeD;
using PokeD.Core.Data.PokeD.Structs;
using PokeD.Core.Data.SCON;
using PokeD.Core.Packets.PokeD.Overworld.Map;

using static Aragas.Network.IO.PacketSerializer;
using static Aragas.Network.IO.PacketDeserializer;

namespace PokeD.Core.Extensions
{
    public static class PacketExtensions
    {
        private static void Extend<T>(Func<PacketDeserializer, int, T> readFunc, Action<PacketSerializer, T, bool> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        public static void Init()
        {
            Extend<Vector2>(ReadVector2, WriteVector2);
            Extend<Vector3>(ReadVector3, WriteVector3);

            Extend<FileHash[]>(ReadFileHashArray, WriteFileHashArray);
            Extend<FileHash>(ReadFileHash, WriteFileHash);

            Extend<ImageResponse[]>(ReadImageResponseArray, WriteImageResponseArray);
            Extend<TileSetResponse[]>(ReadTileSetResponseArray, WriteTileSetResponseArray);
            Extend<ImageResponse>(ReadImageResponse, WriteImageResponse);
            Extend<TileSetResponse>(ReadTileSetResponse, WriteTileSetResponse);

            Extend<Attack>(ReadAttack, WriteAttack);
            //Extend<MonsterMoves>(ReadMonsterMoves, WriteMonsterMoves);
            Extend<Stats>(ReadStats, WriteStats);
            Extend<Monster>(ReadMonster, WriteMonster);

            //Extend<IMonsterBaseInfo>(ReadIMonsterBaseInfo, WriteIMonsterBaseInfo);

            Extend<CatchInfo>(ReadCatchInfo, WriteCatchInfo);

            Extend<MonsterTeam>(ReadMonsterTeam, WriteMonsterTeam);
            //Extend<MonsterParty>(ReadMonsterParty, WriteIOpponentTeam);
            
            Extend<MetaSwitch>(ReadMetaSwitch, WriteMetaSwitch);
            Extend<MetaPosition>(ReadMetaPosition, WriteMetaPosition);
            Extend<MetaAttack>(ReadMetaAttack, WriteMetaAttack);
            Extend<MetaItem>(ReadMetaItem, WriteMetaItem);
            Extend<BattleState>(ReadBattleState, WriteBattleState);
            
            Extend<Ban>(ReadBan, WriteBan);
            Extend<Log>(ReadLog, WriteLog);
            Extend<PlayerDatabase>(ReadPlayerDatabase, WritePlayerDatabase);
            Extend<PlayerInfo>(ReadPlayerInfo, WritePlayerInfo);

            Extend<Ban[]>(ReadBanArray, WriteBanArray);
            Extend<Log[]>(ReadLogArray, WriteLogArray);
            Extend<PlayerDatabase[]>(ReadPlayerDatabaseArray, WritePlayerDatabaseArray);
            Extend<PlayerInfo[]>(ReadPlayerInfoArray, WritePlayerInfoArray);
        }


        private static void WriteVector2(PacketSerializer serializer, Vector2 value, bool writeDefaultLength = true)
        {
            serializer.Write(value.X);
            serializer.Write(value.Y);
        }
        private static Vector2 ReadVector2(PacketDeserializer deserializer, int length = 0) => new Vector2(deserializer.Read<float>(), deserializer.Read<float>());

        private static void WriteVector3(PacketSerializer serializer, Vector3 value, bool writeDefaultLength = true)
        {
            serializer.Write(value.X);
            serializer.Write(value.Y);
            serializer.Write(value.Z);
        }
        private static Vector3 ReadVector3(PacketDeserializer deserializer, int length = 0) => new Vector3(deserializer.Read<float>(), deserializer.Read<float>(), deserializer.Read<float>());


        private static void WriteFileHashArray(PacketSerializer serializer, FileHash[] value, bool writeDefaultLength = true)
        {
            serializer.Write(new VarInt(value.Length));

            foreach (var fileHash in value)
                serializer.Write(fileHash);
        }
        private static FileHash[] ReadFileHashArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();

            var array = new FileHash[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadFileHash(deserializer);

            return array;
        }

        private static void WriteFileHash(PacketSerializer serializer, FileHash value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Name);
            serializer.Write(value.Hash);
        }
        private static FileHash ReadFileHash(PacketDeserializer deserializer, int length = 0)
        {
            return new FileHash() { Name = deserializer.Read<string>(), Hash = deserializer.Read<string>() };
        }

        
        private static void WriteImageResponseArray(PacketSerializer serializer, ImageResponse[] value, bool writeDefaultLength = true)
        {
            serializer.Write(new VarInt(value.Length));

            foreach (var imageResponse in value)
                serializer.Write(imageResponse);
        }
        private static ImageResponse[] ReadImageResponseArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();

            var array = new ImageResponse[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadImageResponse(deserializer);

            return array;
        }

        private static void WriteTileSetResponseArray(PacketSerializer serializer, TileSetResponse[] value, bool writeDefaultLength = true)
        {
            serializer.Write(new VarInt(value.Length));

            foreach (var tileSetResponse in value)
                serializer.Write(tileSetResponse);
        }
        private static TileSetResponse[] ReadTileSetResponseArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();

            var array = new TileSetResponse[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadTileSetResponse(deserializer);

            return array;
        }

        private static void WriteImageResponse(PacketSerializer serializer, ImageResponse value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Name);
            serializer.Write(value.ImageData);
        }
        private static ImageResponse ReadImageResponse(PacketDeserializer deserializer, int length = 0)
        {
            return new ImageResponse() { Name = deserializer.Read<string>(), ImageData = deserializer.Read<byte[]>() };
        }

        private static void WriteTileSetResponse(PacketSerializer serializer, TileSetResponse value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Name);
            serializer.Write(value.TileSetData);
        }
        private static TileSetResponse ReadTileSetResponse(PacketDeserializer deserializer, int length = 0)
        {
            return new TileSetResponse() { Name = deserializer.Read<string>(), TileSetData = deserializer.Read<string>() };
        }


        #region MonsterInstanceData

        private static void WriteAttack(PacketSerializer serializer, Attack value, bool writeDefaultLength = true)
        {
            serializer.Write(value.StaticData.ID);
            serializer.Write(value.PPCurrent);
            serializer.Write(value.PPUps);
        }
        private static Attack ReadAttack(PacketDeserializer deserializer, int length = 0)
        {
            return new Attack(deserializer.Read<short>(), deserializer.Read<byte>(), deserializer.Read<byte>());
        }

        /*
        private static void WriteMonsterMoves(PacketStream stream, MonsterMoves value, bool writeDefaultLength = true)
        {
            stream.Write(value.Move_0);
            stream.Write(value.Move_1);
            stream.Write(value.Move_2);
            stream.Write(value.Move_3);
        }
        private static MonsterMoves ReadMonsterMoves(PacketDataReader reader, int length = 0)
        {
            return new MonsterMoves(
                reader.Read<MonsterMove>(),
                reader.Read<MonsterMove>(),
                reader.Read<MonsterMove>(),
                reader.Read<MonsterMove>());
        }
        */

        private static void WriteStats(PacketSerializer serializer, Stats value, bool writeDefaultLength = true)
        {
            serializer.Write(value.HP);
            serializer.Write(value.Attack);
            serializer.Write(value.Defense);
            serializer.Write(value.SpecialAttack);
            serializer.Write(value.SpecialDefense);
            serializer.Write(value.Speed);
        }
        private static Stats ReadStats(PacketDeserializer deserializer, int length = 0)
        {
            return new Stats(
                deserializer.Read<short>(),
                deserializer.Read<short>(),
                deserializer.Read<short>(),
                deserializer.Read<short>(),
                deserializer.Read<short>(),
                deserializer.Read<short>());
        }

        private static void WriteCatchInfo(PacketSerializer serializer, CatchInfo value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Method);
            serializer.Write(value.Location);
            serializer.Write(value.TrainerName);
            serializer.Write(value.TrainerID);
            serializer.Write(value.PokeballID);
            serializer.Write(value.Nickname);
        }
        private static CatchInfo ReadCatchInfo(PacketDeserializer deserializer, int length = 0)
        {
            return new CatchInfo
            {
                Method = deserializer.Read<string>(),
                Location = deserializer.Read<string>(),
                TrainerName = deserializer.Read<string>(),
                TrainerID = deserializer.Read<ushort>(),
                PokeballID = deserializer.Read<byte>(),
                Nickname = deserializer.Read<string>()
            };
        }

        private static void WriteMonster(PacketSerializer serializer, Monster value, bool writeDefaultLength = true)
        {
            /*
            stream.Write(value.StaticData.ID);
            stream.Write(value.SecretID);
            stream.Write(value.PersonalityValue);
            stream.Write(value.Nature);
            stream.Write(value.CatchInfo);
            stream.Write(value.Experience);
            stream.Write(value.EggSteps);
            stream.Write(value.IV);
            stream.Write(value.EV);
            stream.Write(value.CurrentHP);
            stream.Write(value.StatusEffect);
            stream.Write(value.Affection);
            stream.Write(value.Friendship);
            stream.Write(value.Moves);
            stream.Write(value.HeldItem);
            */
        }
        private static Monster ReadMonster(PacketDeserializer deserializer, int length = 0)
        {
            return null;
            /*
            var species = reader.Read<short>();
            var secretID = reader.Read<ushort>();
            var personalityValue = reader.Read<uint>();
            var nature = reader.Read<byte>();

            return new Monster(species, secretID, personalityValue, nature)
            {
                CatchInfo = reader.Read<CatchInfo>(),
                Experience = reader.Read<int>(),
                EggSteps = reader.Read<int>(),
                IV = reader.Read<Stats>(),
                EV = reader.Read<Stats>(),
                CurrentHP = reader.Read<short>(),
                StatusEffect = reader.Read<short>(),
                Affection = reader.Read<byte>(),
                Friendship = reader.Read<byte>(),
                Moves = reader.Read<Moves>(),
                HeldItem = reader.Read<short>(),
            };
            */
        }

        #endregion MonsterInstanceData



        private static void WriteMonsterTeam(PacketSerializer serializer, MonsterTeam value, bool writeDefaultLength = true) { }
        private static MonsterTeam ReadMonsterTeam(PacketDeserializer deserializer, int length = 0) { return default; }


        private static void WriteMetaSwitch(PacketSerializer serializer, MetaSwitch value, bool writeDefaultLength = true) { serializer.Write(value.Meta); }
        private static MetaSwitch ReadMetaSwitch(PacketDeserializer deserializer, int length = 0) => new MetaSwitch(deserializer.Read<byte>());

        private static void WriteMetaPosition(PacketSerializer serializer, MetaPosition value, bool writeDefaultLength = true) { serializer.Write(value.Meta); }
        private static MetaPosition ReadMetaPosition(PacketDeserializer deserializer, int length = 0) => new MetaPosition(deserializer.Read<long>());

        private static void WriteBattleState(PacketSerializer serializer, BattleState value, bool writeDefaultLength = true) {  }
        private static BattleState ReadBattleState(PacketDeserializer deserializer, int length = 0) { return default; }

        private static void WriteMetaAttack(PacketSerializer serializer, MetaAttack value, bool writeDefaultLength = true) { serializer.Write(value.Meta); }
        private static MetaAttack ReadMetaAttack(PacketDeserializer deserializer, int length = 0) => new MetaAttack(deserializer.Read<byte>());

        private static void WriteMetaItem(PacketSerializer serializer, MetaItem value, bool writeDefaultLength = true) { serializer.Write(value.Meta); }
        private static MetaItem ReadMetaItem(PacketDeserializer deserializer, int length = 0) => new MetaItem(deserializer.Read<short>());


        private static void WriteBan(PacketSerializer serializer, Ban value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Name);
            serializer.Write(value.IP);
            serializer.Write(value.BanTime);
            serializer.Write(value.UnBanTime);
            serializer.Write(value.Reason);
        }
        private static Ban ReadBan(PacketDeserializer deserializer, int length = 0)
        {
            var value = new Ban();
            value.Name = deserializer.Read(value.Name);
            value.IP = deserializer.Read(value.IP);
            value.BanTime = deserializer.Read(value.BanTime);
            value.UnBanTime = deserializer.Read(value.UnBanTime);
            value.Reason = deserializer.Read(value.Reason);

            return value;
        }

        private static void WriteLog(PacketSerializer serializer, Log value, bool writeDefaultLength = true)
        {
            serializer.Write(value.LogFileName);
        }
        private static Log ReadLog(PacketDeserializer deserializer, int length = 0)
        {
            var value = new Log();
            value.LogFileName = deserializer.Read(value.LogFileName);

            return value;
        }

        private static void WritePlayerDatabase(PacketSerializer serializer, PlayerDatabase value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Name);
            serializer.Write(value.LastIP);
            serializer.Write(value.LastSeen);
        }
        private static PlayerDatabase ReadPlayerDatabase(PacketDeserializer deserializer, int length = 0)
        {
            var value = new PlayerDatabase();
            value.Name = deserializer.Read(value.Name);
            value.LastIP = deserializer.Read(value.LastIP);
            value.LastSeen = deserializer.Read(value.LastSeen);

            return value;
        }

        private static void WritePlayerInfo(PacketSerializer serializer, PlayerInfo value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Name);
            serializer.Write(value.IP);
            serializer.Write(value.Ping);
            serializer.Write(value.Position);
            serializer.Write(value.LevelFile);
            serializer.Write(value.PlayTime);
        }
        private static PlayerInfo ReadPlayerInfo(PacketDeserializer deserializer, int length = 0)
        {
            var value = new PlayerInfo();
            value.Name = deserializer.Read(value.Name);
            value.IP = deserializer.Read(value.IP);
            value.Ping = deserializer.Read(value.Ping);
            value.Position = deserializer.Read(value.Position);
            value.LevelFile = deserializer.Read(value.LevelFile);
            value.PlayTime = deserializer.Read(value.PlayTime);

            return value;
        }


        private static void WriteBanArray(PacketSerializer serializer, Ban[] value, bool writeDefaultLength = true)
        {
            serializer.Write(new VarInt(value.Length));

            foreach (var playerInfo in value)
                serializer.Write(playerInfo);
        }
        private static Ban[] ReadBanArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();

            var array = new Ban[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadBan(deserializer);

            return array;
        }

        private static void WriteLogArray(PacketSerializer serializer, Log[] value, bool writeDefaultLength = true)
        {
            serializer.Write(new VarInt(value.Length));

            foreach (var playerInfo in value)
                serializer.Write(playerInfo);
        }
        private static Log[] ReadLogArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();

            var array = new Log[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadLog(deserializer);

            return array;
        }

        private static void WritePlayerDatabaseArray(PacketSerializer serializer, PlayerDatabase[] value, bool writeDefaultLength = true)
        {
            serializer.Write(new VarInt(value.Length));

            foreach (var playerInfo in value)
                serializer.Write(playerInfo);
        }
        private static PlayerDatabase[] ReadPlayerDatabaseArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();

            var array = new PlayerDatabase[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadPlayerDatabase(deserializer);

            return array;
        }

        private static void WritePlayerInfoArray(PacketSerializer serializer, PlayerInfo[] value, bool writeDefaultLength = true)
        {
            serializer.Write(new VarInt(value.Length));

            foreach (var playerInfo in value)
                serializer.Write(playerInfo);
        }
        private static PlayerInfo[] ReadPlayerInfoArray(PacketDeserializer deserializer, int length = 0)
        {
            if (length == 0)
                length = deserializer.Read<VarInt>();

            var array = new PlayerInfo[length];

            for (var i = 0; i < length; i++)
                array[i] = ReadPlayerInfo(deserializer);

            return array;
        }
    }
}