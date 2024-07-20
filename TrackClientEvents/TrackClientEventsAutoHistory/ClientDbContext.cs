using EntityFrameworkCore.AutoHistory;
using EntityFrameworkCore.AutoHistory.Attributes;
using EntityFrameworkCore.AutoHistory.Extensions;
using Microsoft.EntityFrameworkCore;

namespace TrackClientEventsAutoHistory;

public class CustomAutoHistory : AutoHistory
{
    public required string UserId { get; set; }
}

public class ClientDbContext : DbContext
{
    public DbSet<Client> Client { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=TrackClientEventsAutoHistory;User ID=sa;Password=P@ssw0rd");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.EnableAutoHistory<CustomAutoHistory>();
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var userId = await GetUserId();
        this.EnsureAutoHistory(() => new CustomAutoHistory
        {
            UserId = userId
        });
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> GetUserId()
    {
        await Task.Delay(100);
        return "abc";
    }
}

public class Client
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ClientStatus Status { get; set; }

    [ExcludeFromHistory] public DateTime? Birthday { get; set; }
}

public enum ClientStatus
{
    Active,
    Inactive
}