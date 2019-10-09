using fNbt;
using System;
using System.Runtime.CompilerServices;

namespace MineLib.Protocol5.Data
{
    public struct ItemSlot : IEquatable<ItemSlot>
    {
        public static ItemSlot Empty => new ItemSlot(-1);

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                if (_id == -1)
                {
                    _count = 0;
                    Nbt = null;
                }
            }
        }

        private sbyte _count;
        public sbyte Count
        {
            get => _count;
            set
            {
                _count = value;
                if (_count == 0)
                {
                    _id = -1;
                    Nbt = null;
                }
            }
        }

        public NbtCompound Nbt { get; set; }

        public bool IsEmpty => Id == -1;

        public ItemSlot(int id) : this()
        {
            _id = id;
            _count = 1;
            Nbt = null;
        }

        public ItemSlot(int id, sbyte count) : this(id)
        {
            Count = count;
        }

        public ItemSlot(int id, sbyte count, NbtCompound nbt) : this(id, count)
        {
            Nbt = nbt;
            if (Count == 0)
            {
                Id = -1;
                Nbt = null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ItemSlot itemSlot && Equals(itemSlot);
        public bool Equals(ItemSlot other) => other._id.Equals(_id) && other._count.Equals(_count) && other.Nbt.Equals(Nbt);

        public override int GetHashCode() => HashCode.Combine(_id, _count, Nbt);
    }
}