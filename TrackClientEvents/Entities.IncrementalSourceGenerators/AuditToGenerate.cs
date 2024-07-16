using System.Collections;
using System.Collections.Generic;

namespace Entities.IncrementalSourceGenerators;

public readonly record struct AuditToGenerate
{
    public readonly string Name;
    public readonly string Namespace;
    public readonly EquatableArray<(string Key, PropertyValueOption Value)> PropertyTypes;

    public AuditToGenerate(string Name, List<(string, PropertyValueOption)> properties, string ns)
    {
        this.Name = Name;
        Namespace = ns;
        PropertyTypes = new EquatableArray<(string, PropertyValueOption)>(properties.ToArray());
    }
}

public readonly record struct AuditToGenerate2
{
    public readonly EquatableArray<(string Key, string Value)> PropertyTypes;

    public AuditToGenerate2(List<(string, string)> properties)
    {
        PropertyTypes = new EquatableArray<(string, string)>(properties.ToArray());
    }

}