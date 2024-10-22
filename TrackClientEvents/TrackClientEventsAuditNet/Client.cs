namespace TrackClientEventsAuditNet;

public class Client
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ClientStatus Status { get; set; }
    public DateTime? Birthday { get; set; }
}