using Host.Registrations;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;

namespace Host.Tests.Units;

public class RegistrationFixture : IDisposable
{
    private string _dbPath;
    private readonly string _dbConnectionString;

    public RegistrationFixture()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), "registrations.db");
        _dbConnectionString = $"Data Source={_dbPath}";
    }

    public RegistrationDbContext GetDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<RegistrationDbContext>();
        optionsBuilder.UseSqlite(_dbConnectionString).EnableSensitiveDataLogging();
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

    public QueryRequest BuildQueryRequest(string query, Dictionary<string, object> variables)
    {
        var request = new QueryRequest(new QuerySourceText(query), null, null, null, variables);
        return request;
    }

    public string GetConnectionString() => _dbConnectionString;
}