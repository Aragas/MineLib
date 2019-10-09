using System;
using System.Numerics;

namespace MineLib.Protocol5.Data
{
    public struct Modifiers : IEquatable<Modifiers>
    {
        public BigInteger UUID;
        public double Amount;
        public sbyte Operation;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Modifiers) obj);
        }
        public bool Equals(Modifiers other) => UUID.Equals(other.UUID) && Amount.Equals(other.Amount) && Operation.Equals(other.Operation);

        public override int GetHashCode() => UUID.GetHashCode() ^ Amount.GetHashCode() ^ Operation.GetHashCode();
    }

    public struct EntityProperty : IEquatable<EntityProperty>
    {
        public string Key;
        public double Value;
        public Modifiers[] Modifiers;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((EntityProperty) obj);
        }
        public bool Equals(EntityProperty other) => Key.Equals(other.Key) && Value.Equals(other.Value) && Modifiers.Equals(other.Modifiers);

        public override int GetHashCode() => Key.GetHashCode() ^ Value.GetHashCode() ^ Modifiers.GetHashCode();
    }
}