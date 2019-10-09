using fNbt;

using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Extensions
{
    public static class SlotExtension
    {
        public static short GetMetadata(this in ItemSlot slot) => slot.Nbt != null ? slot.Nbt.TryGet("metadata", out NbtShort result) ? result.Value : (short) 0 : (short) 0;
        public static void SetMetadata(this in ItemSlot slot, short metadata)
        {
            if (slot.Nbt == null)
                return;

            var tag = slot.Nbt.Get<NbtShort>("metadata");
            if (tag != null)
                tag.Value = metadata;
            else
                slot.Nbt.Add(new NbtShort("metadata", metadata));
        }
    }
}