using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Aragas.Network.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AragasNetworkAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AN001";

        private const string Title = "Classes that implement PacketWithAttribute<> should have a [Packet()] attribute.";
        internal const string MessageFormat = "Class '{0}' implements PacketWithAttribute<>. Classes should be using [[Packet()] attribute.";
        private const string Description = "";
        private const string Category = "CodeRules";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            true,
            Description);

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

                if (foundPacketWithAttrribute && !namedTypeSymbol.GetAttributes().Any(att => att.AttributeClass.Name == "PacketAttribute"))
                {
                    var symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
                    var dict = ImmutableDictionary.CreateRange(new KeyValuePair<string, string>[]
                    {
                        new KeyValuePair<string, string>("TypeArg", baseClass.TypeArguments[0].ToDisplayString(symbolDisplayFormat))
                    });
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], dict, namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}