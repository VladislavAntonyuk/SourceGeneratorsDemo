using Entities;

namespace TrackClientEventsAuditNetWithIncrementalSourceGenerators;

[Audit]
public class Client
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ClientStatus Status { get; set; }
    public DateTime? Birthday { get; set; }
}

[Audit]
public class Client2
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ClientStatus Status { get; set; }
    public DateTime? Birthday { get; set; }
}

public class Client3
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ClientStatus Status { get; set; }
    public DateTime Birthday { get; set; }
}