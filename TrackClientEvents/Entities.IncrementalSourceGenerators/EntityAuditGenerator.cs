using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Entities.IncrementalSourceGenerators;

[Generator]
public class AuditGenerator : IIncrementalGenerator
{
	private const string AuditAttribute = "Entities.AuditAttribute";
	private const string AuditAttribute2 = "Entities.AuditAttribute";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx =>
		{
			ctx.AddSource("AuditAttribute.g.cs", SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8));
			ctx.AddSource("Auditable.g.cs", SourceText.From(SourceGenerationHelper.AuditClass, Encoding.UTF8));
		});

		IncrementalValuesProvider<AuditToGenerate?> auditableTypesToGenerate = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				AuditAttribute,
				predicate: (node, _) => node is ClassDeclarationSyntax,
				transform: GetTypeToGenerate);

		var auditableTypes = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				AuditAttribute,
				predicate: (node, _) => node is ClassDeclarationSyntax,
				transform: GetTypeToGenerate2).Collect();

		context.RegisterSourceOutput(auditableTypesToGenerate, static (spc, auditToGenerate) =>
		{
			Execute(in auditToGenerate, spc);
		});

		context.RegisterImplementationSourceOutput(auditableTypes, (ctx, source) =>
		{
			var types = new List<(string originalClassName, string generatedClassName)>();
			foreach (var s in source.SelectMany(x => x.Value.PropertyTypes))
			{
				types.Add((s.Key, s.Value));
			}

			var dbContextCode = SourceGenerationHelper.GenerateDbContext(types.Select(x => x.generatedClassName).ToList());
			ctx.AddSource("ClientDbContext.g.cs", SourceText.From(dbContextCode, Encoding.UTF8));

			var auditMappingExtensions = SourceGenerationHelper.GenerateAuditMappingExtensions(types);
			ctx.AddSource("AuditMappingExtensions.g.cs", SourceText.From(auditMappingExtensions, Encoding.UTF8));
		});
	}

	private AuditToGenerate2? GetTypeToGenerate2(GeneratorAttributeSyntaxContext context, CancellationToken arg2)
	{
		var item = context.SemanticModel.GetDeclaredSymbol(context.TargetNode);
		return new AuditToGenerate2(new List<(string, string)>()
		{
			(item.Name, item.Name + "Auditable")
		});
	}

	static void Execute(in AuditToGenerate? auditToGenerate, SourceProductionContext context)
	{
		if (auditToGenerate is { } eg)
		{
			var result = SourceGenerationHelper.GetAuditableClassCode(eg);
			context.AddSource($"{eg.Name}Auditable.g.cs", SourceText.From(result, Encoding.UTF8));
		}
	}


	static AuditToGenerate? GetTypeToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
	{
		if (context.TargetSymbol is not INamedTypeSymbol symbol)
		{
			return null;
		}

		ct.ThrowIfCancellationRequested();

		var properties = new List<(string, PropertyValueOption)>();
		foreach (var member in symbol.GetMembers())
		{
			if (member is IPropertySymbol fieldSymbol)
			{
				var propertyName = fieldSymbol.Name;
				var propertyType = fieldSymbol.Type.ToDisplayString();
				properties.Add((propertyName, new PropertyValueOption(propertyType, propertyName)));
			}
		}

		return new AuditToGenerate(symbol.Name, properties, symbol.ContainingNamespace.Name);
	}
}