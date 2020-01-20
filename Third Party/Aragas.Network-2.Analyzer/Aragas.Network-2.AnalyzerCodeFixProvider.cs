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

namespace Aragas.Network.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AragasNetwork_2AnalyzerCodeFixProvider)), Shared]
    public class AragasNetwork_2AnalyzerCodeFixProvider : CodeFixProvider
    {
        private const string title = "Add Packet attribute to PacketWithAttribute";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticRuleIds.PacketAttribute);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title,
                    c => AddAttributeAsync(context.Document, declaration, c),
                    title),
                diagnostic);
        }

        private async Task<Solution> AddAttributeAsync(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var attributes = typeDecl.AttributeLists.Add(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("Packet"))))
                .NormalizeWhitespace());

            return document.WithSyntaxRoot(root.ReplaceNode(typeDecl, typeDecl.WithAttributeLists(attributes))).Project.Solution;
        }
    }
}
