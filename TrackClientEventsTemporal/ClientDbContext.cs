using Microsoft.EntityFrameworkCore;

namespace TrackClientEventsTemporal;

public class ClientDbContext : DbContext
{
    public DbSet<Client> Client { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,6666;Database=TrackClientEventsTemporalTable;User ID=sa;Password=P@ssw0rd");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Client>()
            .ToTable(builder => builder.IsTemporal());
        modelBuilder.Entity<Client>().HasData(new()
        {
            Id = 1, Name = "Client 1", Status = ClientStatus.Active, Birthday = DateTime.Now
        }, new()
        {
            Id = 2, Name = "Client 2", Status = ClientStatus.Inactive, Birthday = DateTime.Now
        }, new()
        {
            Id = 3, Name = "Client 3", Status = ClientStatus.Active, Birthday = DateTime.Now
        }, new()
        {
            Id = 4, Name = "Client 4", Status = ClientStatus.Inactive, Birthday = DateTime.Now
        });
    }
}

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ClientStatus Status { get; set; }
    public DateTime? Birthday { get; set; }
}

public enum ClientStatus
{
    Active,
    Inactive
}