using Clients;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("sqlite");

builder.Services.AddDbContextPool<ClientsDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite(connectionString));

var app = builder.Build();

app.MapGet("clients/{clientId}",
    (int clientId, ClientsDbContext dbContext) =>
        dbContext.Set<Client>().FirstAsync(c => c.Id == clientId));

app.MapGet("clients", async (ClientsDbContext dbContext, HttpRequest request) =>
{
    if (request.Query.TryGetValue("clientIds", out var clientIds))
    {
        var clientIdList = clientIds.ToString().Split(",")
            .Select(x => int.Parse(x));
        Log.Logger.Information("Returning all clients with {Ids}", clientIds);
        
        #region todo
        /*
         below implementation does not return any results.
         
         var result = await dbContext.Set<Client>()
            .Where(clientId => clientIdList.Contains(clientId.Id))
            .ToListAsync();
            
         */
        // todo: fix replace with implementation, that will not load full db before filtering
        #endregion
        
        var allClients = await dbContext.Set<Client>()
            .ToListAsync();
        return allClients.Where(client => clientIdList.Contains(client.Id));
    }
    else
    {
        return await dbContext.Set<Client>()
            .ToListAsync();
    }
});


app.Run();