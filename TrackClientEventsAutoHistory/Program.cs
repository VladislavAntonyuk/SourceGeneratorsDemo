using TrackClientEventsAutoHistory;

await using var dbcontext = new ClientDbContext();
await dbcontext.Database.EnsureDeletedAsync();
await dbcontext.Database.EnsureCreatedAsync();
var newClient = new Client
{
    Name = "Client 5",
    Status = ClientStatus.Active,
    Birthday = DateTime.Today
};
await dbcontext.Client.AddAsync(newClient);
await dbcontext.SaveChangesAsync();
Console.WriteLine("new record with updated name added to History table");
newClient.Name = "Client 6";
dbcontext.Client.Update(newClient);
await dbcontext.SaveChangesAsync();
Console.ReadLine();
Console.WriteLine("Status updated");
newClient.Status = ClientStatus.Inactive;
dbcontext.Client.Update(newClient);
await dbcontext.SaveChangesAsync();
Console.ReadLine();
Console.WriteLine("birthday updated");
newClient.Birthday = new DateTime(2000, 11, 10);
dbcontext.Client.Update(newClient);
await dbcontext.SaveChangesAsync();
Console.ReadLine();
Console.WriteLine("Client deleted");
dbcontext.Client.Remove(newClient);
await dbcontext.SaveChangesAsync();
Console.ReadLine();