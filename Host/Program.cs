using Host.Clients;
using Host.Registrations;
using Microsoft.EntityFrameworkCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("sqlite");

builder.Services.AddDbContextPool<RegistrationDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite(connectionString));

builder.Services.AddHttpClient<IClientsService>(client =>
    {
        client.BaseAddress = new Uri("http://localhost:5001");
    })
    .AddTypedClient(client => RestService.For<IClientsService>(client));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<RegistrationQueries>()
    .RegisterDbContext<RegistrationDbContext>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddType<RegistrationType>()
    .AddDataLoader<ClientsByIdsDataLoader>();

var app = builder.Build();

app.MapGraphQL();
app.Run();

public partial class Program { }