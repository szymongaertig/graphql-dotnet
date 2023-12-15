using Clients;
using Dbinit;
using Host.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var dbsPath = Environment.GetEnvironmentVariable("DBS_PATH");

services.AddDbContext<RegistrationDbContext>(o => o.UseSqlite($"Data Source={dbsPath}/registrations.db"));
services.AddDbContext<ClientsDbContext>(o => o.UseSqlite($"Data Source={dbsPath}/clients.db"));

var serviceProvider = services.BuildServiceProvider();

var registrationDbContext = serviceProvider.GetService<RegistrationDbContext>();
var migrations = registrationDbContext.Database.GetMigrations();


var clientsDbContext = serviceProvider.GetService<ClientsDbContext>();
await clientsDbContext.Database.MigrateAsync();

for (var i = 0; i < 100; i++)
{
    var client = ClientFactory.Create();
    var registration = RegistrationFactory.Create(client.Id);
    registrationDbContext.Add(registration);
    clientsDbContext.Add(client);
}

registrationDbContext.SaveChanges();
clientsDbContext.SaveChanges();






