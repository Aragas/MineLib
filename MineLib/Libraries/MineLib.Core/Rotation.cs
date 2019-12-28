using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MineLib.Core
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Rotation : IEquatable<Rotation>
    {
        public readonly float Pitch;
        public readonly float Yaw;
        public readonly float Roll;

        public Rotation(float pitch, float yaw, float roll) { Pitch = pitch; Yaw = yaw; Roll = roll; }
        public Rotation(double pitch, double yaw, double roll) { Pitch = (float) pitch; Yaw = (float) yaw; Roll = (float) roll; }
        public Rotation(in Rotation rotation) { Pitch = rotation.Pitch; Yaw = rotation.Yaw; Roll = rotation.Roll; }

        /// <summary>
        /// Converts this Rotation to a string.
        /// </summary>
        public override string ToString() => $"Pitch: {Pitch}, Yaw: {Yaw}, Roll: {Roll}";

        public static bool operator ==(in Rotation a, in Rotation b) => a.Pitch == b.Pitch && a.Yaw == b.Yaw && a.Roll == b.Roll;
        public static bool operator !=(in Rotation a, in Rotation b) => !(a == b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Rotation rotation && Equals(rotation);
        public bool Equals(Rotation other) => other.Pitch.Equals(Pitch) && other.Yaw.Equals(Yaw) && other.Roll.Equals(Roll);

        public override int GetHashCode() => HashCode.Combine(Yaw, Pitch, Roll);
    }
}