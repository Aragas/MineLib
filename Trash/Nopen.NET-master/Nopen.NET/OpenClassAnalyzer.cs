using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Nopen.NET
{
  public static class ClassDeclarationSyntaxExtensions
  {
    public const string NESTED_CLASS_DELIMITER = "+";
    public const string NAMESPACE_CLASS_DELIMITER = ".";

    public static string GetFullName(this ClassDeclarationSyntax source)
    {
      Contract.Requires(null != source);

      var items = new List<string>();
      var parent = source.Parent;
      while (parent.IsKind(SyntaxKind.ClassDeclaration))
      {
        var parentClass = parent as ClassDeclarationSyntax;
        Contract.Assert(null != parentClass);
        items.Add(parentClass.Identifier.Text);

        parent = parent.Parent;
      }

      var nameSpace = parent as NamespaceDeclarationSyntax;
      Contract.Assert(null != nameSpace);
      var sb = new StringBuilder().Append(nameSpace.Name).Append(NAMESPACE_CLASS_DELIMITER);
      items.Reverse();
      items.ForEach(i => { sb.Append(i).Append(NESTED_CLASS_DELIMITER); });
      sb.Append(source.Identifier.Text);

      var result = sb.ToString();
      return result;
    }
  }

  /// <summary>
  /// Roslyn analyzer which reports classes that are implicitly declared as being open for extension.
  /// </summary>
  /// <remarks>
  /// Leaving classes accidently open for extension results in brittle libraries and inflexible APIs. This analyzer
  /// will produce a compiler error when such declarations are found. An error is chosen over a warning as the presence
  /// of this analyzer during compilation can only mean an intent to enforce this behavior, in which case warnings are
  /// too easily ignored.
  /// </remarks>
  /// <seealso cref="OpenAttribute"/>
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public sealed class OpenClassAnalyzer : DiagnosticAnalyzer
  {
    private const string Title = "Classes should be explicitly marked sealed, abstract, or [Open]";

    internal const string MessageFormat =
      "Class '{0}' is implicitly open. Classes should be explicitly marked sealed, abstract, or [Open].";

    private const string Description = "C# creates new types as open by default which can be dangerous. "
                                       + "This analyzer ensures that the intent to leave a class open is explicitly declared. "
                                       + "For more information see Item 19 of Effective Java, Third Edition, which also applies to C#.";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
      DiagnosticRuleIds.OpenClass,
      Title,
      MessageFormat,
      "NOpen",
      DiagnosticSeverity.Error,
      true,
      Description
    );

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
      context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
      context.EnableConcurrentExecution();
      context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
    }

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
      var namedTypeSymbol = (INamedTypeSymbol) context.Symbol;

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