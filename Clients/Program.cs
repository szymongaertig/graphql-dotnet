using Clients;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var dbsPath = Environment.GetEnvironmentVariable("DBS_PATH");
builder.Services.AddDbContextPool<ClientsDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite($"Data Source={dbsPath}/clients.db"));

var app = builder.Build();

app.MapGet("/", () => "ok");

app.MapGet("clients/{clientId}",
    async (Guid clientId, ClientsDbContext dbContext) => await dbContext.Set<Client>().FirstAsync(c => c.Id == clientId));

app.MapGet("clients", (ClientsDbContext dbContext) => dbContext.Set<Client>().AsAsyncEnumerable());

app.Run();