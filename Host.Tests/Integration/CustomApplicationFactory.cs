using Host.Registrations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Memory;

namespace Host.Tests.Integration;

public class CustomApplicationFactory : WebApplicationFactory<Program>
{
    private string _dbPath;
    private readonly string _dbConnectionString;

    public CustomApplicationFactory()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.db");
        _dbConnectionString = $"Data Source={_dbPath}";
    }

    private RegistrationDbContext GetDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<RegistrationDbContext>();
        optionsBuilder.UseSqlite(_dbConnectionString).EnableSensitiveDataLogging();
        return new RegistrationDbContext(optionsBuilder.Options);
    }

    protected override IHostBuilder? CreateHostBuilder()
    {
        CreateDb();
        return base.CreateHostBuilder();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var memoryConfigurationSource = new MemoryConfigurationSource()
        {
            InitialData = new[]
            {
                new KeyValuePair<string, string>("ConnectionStrings:sqlite", _dbConnectionString)
            }
        };
        builder.UseConfiguration(new ConfigurationRoot(new List<IConfigurationProvider>()
        {
            new MemoryConfigurationProvider(memoryConfigurationSource)
        }));

        base.ConfigureWebHost(builder);
    }

    private void CreateDb()
    {
        Console.WriteLine($"Creating db on file: {_dbPath}");
        var dbContext = GetDbContext();
        dbContext.Database.Migrate();
    }

    public async Task ArrangeRegistration(Registration registration)
    {
        var dbContext = GetDbContext();
        dbContext.Add(registration);
        await dbContext.SaveChangesAsync();
    }

    public override ValueTask DisposeAsync()
    {
        File.Delete(_dbPath);
        return base.DisposeAsync();
    }
}