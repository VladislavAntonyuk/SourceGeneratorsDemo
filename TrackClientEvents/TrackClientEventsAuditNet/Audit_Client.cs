using System.ComponentModel.DataAnnotations;

namespace TrackClientEventsAuditNet;

public class Audit_Client
{
    [Key] public int Identifier { get; set; }

    public int? Id { get; set; }
    public string? Name { get; set; }
    public ClientStatus Status { get; set; }
    public DateTime AuditDate { get; set; }
    public string UserId { get; set; }
    public string ActionType { get; set; }
}