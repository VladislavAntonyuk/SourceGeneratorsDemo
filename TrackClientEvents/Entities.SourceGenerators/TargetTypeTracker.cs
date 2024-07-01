using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Entities.SourceGenerators;

public class TargetTypeTracker : ISyntaxContextReceiver
{
    public IImmutableList<TypeDeclarationSyntax> AuditableTypes = ImmutableList.Create<TypeDeclarationSyntax>();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is TypeDeclarationSyntax typeDeclarationSyntax)
            if (typeDeclarationSyntax.IsDecoratedWithAttribute("audit"))
                AuditableTypes = AuditableTypes.Add(typeDeclarationSyntax);
    }
}