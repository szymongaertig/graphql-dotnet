using Host.Registrations;
using Host.Tests.Mocks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Host.Tests.Units;

public class WhenGettingRegistration_ExtendedWithClientData
{
    [Fact]
    public async Task ForNotExistingRegistration_ThenReturnsValidRegistrationId()
    {
        // Arrange
        var fixture = new RegistrationFixture();
        var dbContext = fixture.GetDbContext();
        await dbContext.Database.MigrateAsync();
        var registration = new RegistrationMockBuilder().Build();

        var request = fixture.BuildQueryRequest(@"query getRegistrations($registrationId: Int!) {
          registrations(where: {id : {eq: $registrationId }}) {
            nodes {
              id
            }
          }
        }", new Dictionary<string, object>()
        {
            { "registrationId", registration.Id }
        });

        var requestExecutor = await new ServiceCollection()
            .AddDbContextPool<RegistrationDbContext>(optionsBuilder =>
                optionsBuilder.UseSqlite(fixture.GetConnectionString()))
            .AddGraphQLServer()
            .AddRegistartionGraphQL()
            .BuildRequestExecutorAsync();

        // Act
        var result = await requestExecutor.ExecuteAsync(request);

        // Assert
        result.ExpectQueryResult();
        var jsonResult = result.ExpectQueryResult().ToDictionary();
    }
}