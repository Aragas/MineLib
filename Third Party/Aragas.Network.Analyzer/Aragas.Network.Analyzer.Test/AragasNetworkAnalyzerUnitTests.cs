using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using TestHelper;

namespace Aragas.Network.Analyzer.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {
        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace ConsoleApplication1
{
    class CorrectPacket : AbstractCorrectPacket
    {   
        public override void Deserialize(IPacketDeserializer deserializer) { }

        public override void Serialize(IPacketSerializer serializer) { }
    }
    abstract class AbstractCorrectPacket : PacketWithAttribute<object>
    {
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "AN001",
                Message = String.Format("Class '{0}' implements PacketWithAttribute<>. Classes should be using [[Packet()] attribute.", "CorrectPacket"),
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 7, 11) }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.Network.Attributes;

namespace ConsoleApplication1
{
    [Packet(0x00)]
    class CorrectPacket : AbstractCorrectPacket
    {   
        public override void Deserialize(IPacketDeserializer deserializer) { }

        public override void Serialize(IPacketSerializer serializer) { }
    }
    abstract class AbstractCorrectPacket : PacketWithAttribute<object>
    {
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod3()
        {
            var test = @"
using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.Network.Attributes;

namespace ConsoleApplication1
{
    [Packet(0x01)]
    class CorrectPacket : PacketWithAttribute<object>
    {   
        public override void Deserialize(IPacketDeserializer deserializer) { }

        public override void Serialize(IPacketSerializer serializer) { }
    }
}";
            VerifyCSharpDiagnostic(test);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new AragasNetworkAnalyzerCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new AragasNetworkAnalyzerAnalyzer();
    }
}
