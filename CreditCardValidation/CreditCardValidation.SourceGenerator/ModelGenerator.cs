using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CreditCardValidation.SourceGenerator;

[Generator]
public class ModelGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx =>
		{
			ctx.AddSource("PropertyValue.g.cs", SourceText.From(SourceGenerationHelper.GetPropertyValueCode(), Encoding.UTF8));
		});

		IncrementalValuesProvider<ClassToGenerate?> provider = context.SyntaxProvider.CreateSyntaxProvider<ClassToGenerate?>(
			predicate: (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax { BaseList: not null } declaration
										  && declaration.BaseList.Types.Any(type => type.ToString() == "BaseModel"),
			transform: GetTypeToGenerate);

		context.RegisterSourceOutput(provider, static (spc, auditToGenerate) =>
		{
			Execute(in auditToGenerate, spc);
		});
	}

	static void Execute(in ClassToGenerate? auditToGenerate, SourceProductionContext context)
	{
		if (auditToGenerate is { } eg)
		{
			var result = SourceGenerationHelper.GetCode(eg);
			context.AddSource($"{eg.Name}.g.cs", SourceText.From(result, Encoding.UTF8));
		}
	}


	static ClassToGenerate? GetTypeToGenerate(GeneratorSyntaxContext context, CancellationToken ct)
	{
		var symbol = context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol;
		if (symbol is null)
		{
			return null;
		}

		ct.ThrowIfCancellationRequested();

		var properties = new List<string>();
		foreach (var member in (symbol.BaseType?.GetMembers() ?? ImmutableArray<ISymbol>.Empty).Concat(symbol.GetMembers()))
		{
			if (member is IPropertySymbol propertySymbol && propertySymbol.GetAttributes().Any(x => x.AttributeClass?.Name == "ValidateCreditCardAttribute"))
			{
				var propertyName = propertySymbol.Name;
				properties.Add(propertyName);
			}
		}

		return new ClassToGenerate(symbol.Name, properties, symbol.ContainingNamespace.Name);
	}
}