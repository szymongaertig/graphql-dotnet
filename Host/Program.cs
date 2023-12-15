using Host;
using Host.Registrations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<RegistrationDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite("Data Source=../registrations.db"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<RegistrationQueries>();

var app = builder.Build();

app.MapGraphQL();
app.Run();