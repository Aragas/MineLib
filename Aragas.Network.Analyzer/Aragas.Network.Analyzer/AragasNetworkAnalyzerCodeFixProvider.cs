using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Aragas.Network.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AragasNetworkAnalyzerCodeFixProvider)), Shared]
    public class AragasNetworkAnalyzerCodeFixProvider : CodeFixProvider
    {
        private const string title = "Add PacketAttribute";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AragasNetworkAnalyzerAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title,
                        cancellationToken => AddAttributeAsync(context.Document, diagnostic, cancellationToken),
                        title),
                    diagnostic);
            }

            return Task.CompletedTask;
        }

        private static object? GetDefault(Type? type)
        {
            if (type == typeof(string))
            {
                return "\"0x00\"";
            }
            if (type?.IsValueType == true)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        private async Task<Document> AddAttributeAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            //var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            if (syntaxRoot == null || !(syntaxRoot.FindNode(diagnostic.Location.SourceSpan) is TypeDeclarationSyntax typeDeclarationNode))
                return document;

            var typeArgStr = diagnostic.Properties["TypeArg"];
            var typeArg = Type.GetType(typeArgStr);
            var attribute =
                SyntaxFactory.Attribute(
                    SyntaxFactory.ParseName("Packet"),
                    SyntaxFactory.ParseAttributeArgumentList($"({GetDefault(typeArg)?.ToString() ?? "0x00"})"));

            var attributes = typeDeclarationNode.AttributeLists.Add(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(attribute)));

            var newRoot = syntaxRoot.ReplaceNode(typeDeclarationNode, typeDeclarationNode.WithAttributeLists(attributes));

            var usingDirective = SyntaxFactory.UsingDirective(
                SyntaxFactory.ParseName("Aragas.Network.Attributes"));

            if (newRoot is CompilationUnitSyntax compilation && !compilation.Usings.Any(u => u.Name.GetText().ToString() == usingDirective.Name.GetText().ToString()))
            {
                var lastUsings = compilation.Usings.Last();
                newRoot = newRoot.InsertNodesAfter(lastUsings, new[] { usingDirective });
            }

            return document.WithSyntaxRoot(newRoot);
        }
    }
}