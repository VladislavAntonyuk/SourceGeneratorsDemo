using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Entities.SourceGenerators;

internal static class SourceGeneratorExtensions
{
    internal static bool IsDecoratedWithAttribute(this TypeDeclarationSyntax typeDeclarationSyntax,
        string attributeName)
    {
        return typeDeclarationSyntax.AttributeLists
            .SelectMany(x => x.Attributes)
            .Any(x => x.Name.ToString().ToLower() == attributeName);
    }

    internal static string? BuildProperty(this PropertyDeclarationSyntax pds, Compilation compilation)
    {
        var symbol = compilation
            .GetSemanticModel(pds.SyntaxTree)
            .GetDeclaredSymbol(pds);

        return symbol is null ? null : $"public {symbol.Type.Name()} {symbol.Name} {{get; set;}}";
    }

    private static string Name(this ISymbol typeSymbol)
    {
        return typeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
    }
}