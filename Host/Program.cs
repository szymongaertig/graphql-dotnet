using Host.Registrations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbsPath = Environment.GetEnvironmentVariable("DBS_PATH");

//builder.services.AddDbContext
//builder.services.AddDbContextFactory

builder.Services.AddDbContextPool<RegistrationDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite($"Data Source={dbsPath}/registrations.db"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType()
    .AddType<RegistrationQueries>();
    //.RegisterDbContext<RegistrationDbContext>();

var app = builder.Build();

app.MapGraphQL();
app.Run();