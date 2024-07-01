using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Entities.SourceGenerators;

[Generator]
public class AuditGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new TargetTypeTracker());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        context.AddSource("AuditAttribute.g.cs", SourceGenerationHelper.Attribute);
        context.AddSource("Auditable.g.cs", SourceGenerationHelper.AuditClass);

        if (context.SyntaxContextReceiver is not TargetTypeTracker targetTypeTracker) return;

        var types = new List<(string originalClassName, string generatedClassName)>();
        foreach (var typeNode in targetTypeTracker.AuditableTypes)
        {
            var result = SourceGenerationHelper.GetAuditableClassCode(context, typeNode);
            if (result.HasValue)
            {
                var (auditableClassCode, originalClassName, generatedClassName) = result.Value;
                types.Add((originalClassName, generatedClassName));
                context.AddSource($"{generatedClassName}.g.cs", SourceText.From(auditableClassCode, Encoding.UTF8));
            }
        }

        var dbContextCode = SourceGenerationHelper.GenerateDbContext(types.Select(x => x.generatedClassName).ToList());
        context.AddSource("ClientDbContext.g.cs", SourceText.From(dbContextCode, Encoding.UTF8));

        var auditMappingExtensions = SourceGenerationHelper.GenerateAuditMappingExtensions(types);
        context.AddSource("AuditMappingExtensions.g.cs", SourceText.From(auditMappingExtensions, Encoding.UTF8));
    }
}