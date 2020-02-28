using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Aragas.Network.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AragasNetwork_2AnalyzerAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Classes that inherit PacketWithAttribute<> should have [Packet(ID)]";

        internal const string MessageFormat =
          "Class '{0}' is implicitly open. Classes should be explicitly marked sealed, abstract, or [Open].";

        private const string Description = "C# creates new types as open by default which can be dangerous. "
                                           + "This analyzer ensures that the intent to leave a class open is explicitly declared. "
                                           + "For more information see Item 19 of Effective Java, Third Edition, which also applies to C#.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
          DiagnosticRuleIds.PacketAttribute,
          Title,
          MessageFormat,
          "AN2",
          DiagnosticSeverity.Error,
          true,
          Description
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            if (namedTypeSymbol.TypeKind == TypeKind.Class && !namedTypeSymbol.IsAbstract)
            {
                var foundPacketWithAttrribute = false;
                var baseClass = namedTypeSymbol;
                do
                {
                    baseClass = baseClass.BaseType;
                    if (baseClass == null)
                        break;
                    foundPacketWithAttrribute = baseClass.MetadataName == "PacketWithAttribute`1";
                }
                while (!foundPacketWithAttrribute);

                if (foundPacketWithAttrribute && !namedTypeSymbol.GetAttributes().Any(att => att.AttributeClass.Name == "Packet"))
                {
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}