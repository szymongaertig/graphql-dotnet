using Host.Clients;
using Host.Email;
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

builder.Services.AddSingleton<IEmailService, EmailServiceGrpc>();
builder.Services
    .AddGrpcClient<Emails.Email.EmailClient>(o =>
    {
        o.Address = new Uri("http://localhost:5002");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        return handler;
    });

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

var email = await app.Services.GetService<IEmailService>().GetClientEmails(1);
app.MapGraphQL();
app.Run();

public partial class Program { }