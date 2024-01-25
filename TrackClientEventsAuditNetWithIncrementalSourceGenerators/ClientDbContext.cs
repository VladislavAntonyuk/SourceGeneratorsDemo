using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace TrackClientEventsAuditNetWithIncrementalSourceGenerators;

public partial class ClientDbContext : DbContext
{
    public DbSet<Client> Client => Set<Client>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=TrackClientEventsIncrementalSourceGenerators;User ID=sa;Password=P@ssw0rd");
        optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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