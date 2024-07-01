using System.Collections.Generic;
using CreditCardValidation.SourceGenerator.Shared;

namespace CreditCardValidation.SourceGenerator;

public readonly record struct ClassToGenerate
{
    public readonly string Name;
    public readonly string Namespace;
    public readonly EquatableArray<string> Properties;

    public ClassToGenerate(string name, List<string> properties, string ns)
    {
        Name = name;
        Namespace = ns;
        Properties = new EquatableArray<string>(properties.ToArray());
    }

}