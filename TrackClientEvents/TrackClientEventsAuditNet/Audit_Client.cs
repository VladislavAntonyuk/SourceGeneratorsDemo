using System.ComponentModel.DataAnnotations;

namespace TrackClientEventsAuditNet;

public class Audit_Client
{
    [Key] public int Identifier { get; set; }

    public int? Id { get; set; }
    public string? Name { get; set; }
    public ClientStatus Status { get; set; }
    public DateTime AuditDate { get; set; }
    public required string UserId { get; set; }
    public required string ActionType { get; set; }
}