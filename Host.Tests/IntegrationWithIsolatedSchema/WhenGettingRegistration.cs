using FluentAssertions;
using Host.Registrations;
using Host.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Host.Tests.Units;

public class WhenGettingRegistration
{
    [Fact]
    public async Task ForNotExistingRegistration_ThenReturnsValidRegistrationId()
    {
        // Arrange
        using var fixture = new RegistrationFixture();
        var dbContext = fixture.GetDbContext();
        await dbContext.Database.MigrateAsync();
        var registration = new RegistrationMockBuilder().Build();

        // Act
        var jsonResult = await fixture.ExecuteGraphqlRequest(
            @"query getRegistrations($registrationId: Int!) {
          registrations(where: {id : {eq: $registrationId }}) {
            nodes {
              id
            }
          }
        }", new Dictionary<string, object>()
            {
                { "registrationId", registration.Id }
            },
            (services) =>
            {
                services.AddDbContextPool<RegistrationDbContext>(optionsBuilder =>
                    optionsBuilder.UseSqlite(fixture.GetConnectionString()));
            });

        // Assert
        var nodes = (JArray)jsonResult["data"]["registrations"]["nodes"];
        nodes.Should().BeEmpty();
    }

    [Fact]
    public async Task ThenReturnsValidRegistrationId()
    {
        // Arrange
        using var fixture = new RegistrationFixture();
        var dbContext = fixture.GetDbContext();
        await dbContext.Database.MigrateAsync();
        var registration = new RegistrationMockBuilder().Build();

        dbContext.Add(registration);
        dbContext.SaveChanges();

        // Act
        var jsonResult = await fixture.ExecuteGraphqlRequest(
            @"query getRegistrations($registrationId: Int!) {
          registrations(where: {id : {eq: $registrationId }}) {
            nodes {
              id
            }
          }
        }", new Dictionary<string, object>()
            {
                { "registrationId", registration.Id }
            },
            (services) =>
            {
                services.AddDbContextPool<RegistrationDbContext>(optionsBuilder =>
                    optionsBuilder.UseSqlite(fixture.GetConnectionString()));
            });

        // Assert
        var registrationId = jsonResult["data"]["registrations"]["nodes"]?.FirstOrDefault().Value<int>("id");
        registrationId.Should().Be(registration.Id);
    }
}