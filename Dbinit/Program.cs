using Clients;
using Dbinit;
using Host.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDbContext<RegistrationDbContext>(o => o.UseSqlite("Data Source=../registrations.db"));
services.AddDbContext<ClientsDbContext>(o => o.UseSqlite("Data Source=../clients.db"));

var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Creating registrations db");
var registrationDbContext = serviceProvider.GetService<RegistrationDbContext>();
await registrationDbContext.Database.EnsureCreatedAsync();
var registrations = registrationDbContext.Registrations.ToList();

Console.WriteLine("Creating clients db");
var clientsDbContext = serviceProvider.GetService<ClientsDbContext>();


for (var i = 0; i < 100; i++)
{
    var client = ClientFactory.Create();
    var registration = new Registration()
    {
        ClientId = client.Id,
        Id = Guid.NewGuid(),
        CreationDate = DateTime.Now,
        Status = RegistrationStatus.Confirmed
    };
    registrationDbContext.Add(registration);
    clientsDbContext.Add(client);
}

registrationDbContext.SaveChanges();
clientsDbContext.SaveChanges();





