using System;

namespace Aragas.Network
{
    /// <summary>
    /// Basically Nullable without struct constraint
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    internal struct Initializable<T>
    {
        private readonly bool hasInitialized;  // Do not rename (binary serialization)
        private readonly T value; // Do not rename (binary serialization)

        public Initializable(T value)
        {
            this.value = value;
            hasInitialized = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Simplification", "RCS1085:Use auto-implemented property.", Justification = "Binary serialization")]
        public bool HasInitialized
        {
            get => hasInitialized;
        }

        public T Value
        {
            get
            {
                if (!hasInitialized)
                {
                    throw new Exception();
                }
                return value;
            }
        }

        public T GetValueOrDefault() => value;

        public T GetValueOrDefault(T defaultValue) => hasInitialized ? value : defaultValue;

        public override bool Equals(object? other)
        {
            if (!hasInitialized) return other == null;
            if (other == null) return false;
            return value!.Equals(other);
        }

        public override int GetHashCode() => hasInitialized ? value!.GetHashCode() : 0;

        public override string? ToString() => hasInitialized ? value!.ToString() : "";

        public static implicit operator Initializable<T>(T value) => new Initializable<T>(value);

        public static explicit operator T(Initializable<T> value) => value!.Value;
    }
}
