using Aragas.Network.IO;

using System;

namespace PokeD.Core.IO
{
    public class P3DSerializer : StreamSerializer
    {
        public override Span<byte> GetData() => Array.Empty<byte>();

        // -- Anything 
        public override void Write<T>(T value = default, bool writeDefaultLength = true) { }
    }
}