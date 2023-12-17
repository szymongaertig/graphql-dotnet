using Host.Registrations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using HotChocolate;
using HotChocolate.Execution;
using Path = System.IO.Path;

namespace Host.Tests.Units;

public class RegistrationFixture : IDisposable
{
    private string _dbPath;
    private readonly string _dbConnectionString;

    public RegistrationFixture()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.db");
        _dbConnectionString = $"Data Source={_dbPath}";
        var dbContext = GetDbContext();
        dbContext.Database.Migrate();
    }

    public RegistrationDbContext GetDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<RegistrationDbContext>();
        optionsBuilder.UseSqlite(_dbConnectionString);
        return new RegistrationDbContext(optionsBuilder.Options);
    }

    public void Dispose()
    {
        try
        {
            File.Delete(_dbPath);
        }
        catch
        {
        }
    }

    private QueryRequest BuildQueryRequest(string query, Dictionary<string, object> variables)
    {
        var request = new QueryRequest(new QuerySourceText(query), null, null, null, variables);
        return request;
    }

    public string GetConnectionString() => _dbConnectionString;

    public async Task<JObject> ExecuteGraphqlRequest(string query,
        Dictionary<string, object> variables,
        Action<ServiceCollection> serviceInstaller)
    {
        var serviceCollection = new ServiceCollection();

        if (serviceInstaller != null)
        {
            serviceInstaller.Invoke(serviceCollection);
        }

        var executor = await serviceCollection.AddGraphQLServer()
            .AddRegistartionGraphQL()
            .BuildRequestExecutorAsync();
        var request = BuildQueryRequest(query, variables);
        var result = await executor.ExecuteAsync(request);
        result.ExpectQueryResult();
        
        return JObject.Parse(result.ToJson());
    }
}