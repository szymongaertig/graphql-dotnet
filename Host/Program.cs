using Host;
using Host.Email;
using Host.Registrations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("sqlite");

builder.Services.AddDbContextPool<RegistrationDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite(connectionString));

builder.Services.AddClientServiceHttpClient();
builder.Services.AddSingleton<IEmailService, EmailServiceGrpc>();
builder.Services.AddEmailServiceGrpcClient();

builder.Services
    .AddGraphQLServer()
    .AddRegistartionGraphQL();

var app = builder.Build();

app.MapGraphQL();
app.Run();
public partial class Program { }