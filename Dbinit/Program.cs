using Clients;
using Dbinit;
using Host.Events;
using Host.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var services = new ServiceCollection();
var dbsPath = Environment.GetEnvironmentVariable("DBS_PATH");

services.AddDbContext<RegistrationDbContext>(o => 
    o.UseSqlite($"Data Source={dbsPath}/registrations.db").EnableSensitiveDataLogging());
services.AddDbContext<ClientsDbContext>(o => o.UseSqlite($"Data Source={dbsPath}/clients.db")
    .EnableSensitiveDataLogging());

var serviceProvider = services.BuildServiceProvider();
var registrationDbContext = serviceProvider.GetService<RegistrationDbContext>();
var clientsDbContext = serviceProvider.GetService<ClientsDbContext>();

if (await registrationDbContext.Registrations.AnyAsync())
{
    Log.Logger.Information("Database has already been initialized");
    return;
}

for (var eventId = 1; eventId <= 3; eventId++)
{
    var @event = new Event()
    {
        Id = eventId,
        EventName = $"Event {eventId}"
    };
    
    registrationDbContext.Events.Add(@event);
    
    for (var i = (eventId-1)*100+1; i <= eventId*100; i++)
    {
        var client = ClientFactory.Create(i);
        var registration = RegistrationFactory.Create(i);
        registration.Event = @event;
        registrationDbContext.Add(registration);
        clientsDbContext.Add(client);

        registrationDbContext.SaveChanges();
        Log.Logger.Information("Registration {RegistrationId} has been created", i);
    }
}



registrationDbContext.SaveChanges();
clientsDbContext.SaveChanges();
Log.Logger.Information("Changes has been saved");