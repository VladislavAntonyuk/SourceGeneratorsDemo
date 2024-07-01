using System;

namespace Entities.IncrementalSourceGenerators;

public readonly struct PropertyValueOption : IEquatable<PropertyValueOption>
{
    public PropertyValueOption(string ns, string name)
    {
        Type = ns;
        Name = name;
    }

    public string Type { get; }
    public string Name { get; }


    public bool Equals(PropertyValueOption other)
    {
        return Type == other.Type
               && Name == other.Name;
    }
}